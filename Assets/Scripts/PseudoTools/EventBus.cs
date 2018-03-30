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
                    throw new Exception("Cant find the event name in EventBus.");
                }
                if(!ObserverMap[eventName].Contains(observer)) {
                    throw new Exception("Cant find the target observer in EventBus.");
                }
                ObserverMap[eventName].Remove(observer);
            });
        } 
        public static void Notify(string eventName, string eventContent) {
            if(!ObserverMap.ContainsKey(eventName)) {
                throw new Exception("Cant find the event name in EventBus.");
            }
            foreach(var observer in ObserverMap[eventName]) {
                MethodInfo receiveMethod = observer.GetType().GetMethod("Receive" + eventName);
                if(receiveMethod != null) {
                    ParameterInfo[] ps = receiveMethod.GetParameters();
                    if(ps.Length == 1 && ps[0].GetType() == typeof(string)) {
                        object[] invokeParameter = new object[]{eventContent};
                        receiveMethod.Invoke(observer, invokeParameter);
                    }
                }
            }
        }
    }
    public abstract class PObserverMonoBehaviour : MonoBehaviour {
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