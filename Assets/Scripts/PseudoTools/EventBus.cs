using System;
using System.Collections.Generic;
using System.Reflection;
namespace PseudoTools {
    public class EventBus {
        private static EventBus instance = null;
        private static Dictionary<string, List<object>> ObserverMap{
            get{
                if(instance == null) {
                    instance = new EventBus();
                }
                return instance.observerMap;
            }
        }
        public Dictionary<string, List<object>> observerMap = new Dictionary<string, List<object>>();
        //
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
                    //throw new Exception("Cant find the observer in EventBus.");
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
                foreach(var method in observer.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                    if(method.Name == "Receive" + eventName && sameTypeParameters(method.GetParameters(), eventContents)) {
                        method.Invoke(observer, eventContents);
                    }
                }
            }
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class ReceiveEvent: System.Attribute
    {
        readonly string eventName;
        
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