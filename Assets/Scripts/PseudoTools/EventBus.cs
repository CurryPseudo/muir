using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace PseudoTools {
    public class EventBus {
        private static EventBus instance = null;
        public static Dictionary<string, List<object>> ObserverMap{
            get{
                if(instance == null) {
                    instance = new EventBus();
                }
                return instance.observerMap;
            }
        }
        public Dictionary<string, List<object>> observerMap = new Dictionary<string, List<object>>();
        public static void Register(object observer) {
            Action<ReceiveEvent> addMap = (re) => {
                var eventName = re.EventName;
                if(!ObserverMap.ContainsKey(eventName)) {
                    ObserverMap.Add(eventName, new List<object>());
                }
                ObserverMap[eventName].Add(observer);
            };
            mapAction(observer, addMap);
        }
        private static void mapAction(object observer, Action<ReceiveEvent> action) {
            Type observerType = observer.GetType();
            Attribute[] attributes = Attribute.GetCustomAttributes(observerType);
            foreach(var attrs in attributes) {
                if(attrs is ReceiveEvent) {
                    action((ReceiveEvent)attrs);       
                }
            }
        }
        public static void Deregister(object observer) {
            mapAction(observer, (re) => {
                var eventName = re.EventName;
                if(!ObserverMap.ContainsKey(eventName)) {
                    //throw new Exception("Cant find the event name in EventBus.");
                    return;
                }
                if(!ObserverMap[eventName].Contains(observer)) {
                    return;
                }
                ObserverMap[eventName].Remove(observer);
            });
        } 
        public static void Notify(string eventName, params object[] eventContents) {
            if(!ObserverMap.ContainsKey(eventName)) {
                return;
            }
            foreach(var observer in ObserverMap[eventName]) {
                Func<ParameterInfo[], object[], bool> sameTypeParameters = (ps, os) => {
                    if(ps.Length != os.Length) {
                        return false;
                    }
                    for(int i = 0; i < ps.Length; i++) {
                        if(ps[i].ParameterType != os[i].GetType()) {
                            return false;
                        }
                    }
                    return true;
                };
                foreach(var method in observer.GetType().GetMethods()) {
                    if(method.Name == "Receive" + eventName && sameTypeParameters(method.GetParameters(), eventContents)) {
                        method.Invoke(observer, eventContents);
                    }
                }
            }
        }
    }
    public abstract class ObserverMonoBehaviour : MonoBehaviour {
       public void OnEnable() {
           EventBus.Register(this);
           _OnEnable();
       }
       public abstract void _OnEnable();
       public void OnDisable() {
           EventBus.Deregister(this);
           _OnDisable();
       }
       public abstract void _OnDisable();
    }
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class ReceiveEvent: System.Attribute
    {
        // See the attribute guidelines at
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string eventName;
        
        // This is a positional argument
        public ReceiveEvent(string eventName)
        {
            this.eventName = eventName;
        }
        
        public string EventName
        {
            get { return eventName; }
        }
        
    }
}