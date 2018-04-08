using System;
public class ActionEvent<T> {
    public event Action<T> Event;
    public void Invoke(T t) {
        if(Event != null) {
            Event.Invoke(t);
        }
    }
}
public class ActionEvent{
    public event Action Event;
    public void Invoke() {
        if(Event != null) {
            Event.Invoke();
        }
    }
}