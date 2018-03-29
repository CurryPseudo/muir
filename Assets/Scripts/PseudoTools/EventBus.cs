using System;
using System.Collections.Generic;
namespace PseudoTools {
    public class EventBus {
        private static EventBus instance = null;
        public static Dictionary<string, List<IPObserver>> ObserverMap{
            get{
                if(instance == null) {
                    instance = new EventBus();
                }
                return instance.observerMap;
            }
        }
        public Dictionary<string, List<IPObserver>> observerMap = new Dictionary<string, List<IPObserver>>();
        public static void Register(IPObserver observer, string eventName) {
            if(!ObserverMap.ContainsKey(eventName)) {
                ObserverMap.Add(eventName, new List<IPObserver>());
            }
            ObserverMap[eventName].Add(observer);
        }
        public static void Deregister(IPObserver observer, string eventName) {
            if(!ObserverMap.ContainsKey(eventName)) {
                throw new Exception("Cant find the event name in EventBus.");
            }
            if(!ObserverMap[eventName].Contains(observer)) {
                throw new Exception("Cant find the target observer in EventBus.");
            }
            ObserverMap[eventName].Remove(observer);
        } 
        public static void Notify(string eventName, string eventContent) {
            if(!ObserverMap.ContainsKey(eventName)) {
                throw new Exception("Cant find the event name in EventBus.");
            }
            foreach(var observer in ObserverMap[eventName]) {
                observer.ReceiveEvent(eventName, eventContent);
            }
        }
    }
    public interface IPObserver {
        void ReceiveEvent(string eventName, string eventContent = ""); 
    }
}