using System.Collections;
using System.Collections.Generic;
public class ActionEventMap<T> {
    Dictionary<T, ActionEvent> eventMap = new Dictionary<T, ActionEvent>();
		public void AddEnterEvent(T t, System.Action action) {
			if(!eventMap.ContainsKey(t)) {
				eventMap.Add(t, new ActionEvent());
			}
			eventMap[t].Event += action;
		}
        public void AddEnterEvent(T t, System.Action action, MonoBehaviourHasDestroyEvent mb) {
            AddEnterEvent(t, action);
            mb.onDestroy += () => {
                RemoveEnterEvent(t, action);
            };
        }
        public void RemoveEnterEvent(T t, System.Action action) {
            if(!eventMap.ContainsKey(t)) {
                throw new System.Exception("Event map doesn't contain needed key.");
            }
            eventMap[t].Event -= action;
        }
        public void Invoke(T t) {
            if(eventMap.ContainsKey(t)){
                eventMap[t].Invoke();
            }
        }
}