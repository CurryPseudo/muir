using System;
public interface IEnterEvents<T> {
    void AddEnterEvent(T t, Action action);
    void AddEnterEvent(T t, Action action, MonoBehaviourHasDestroyEvent mb);
    void RemoveEnterEvent(T t, Action action);
    void Invoke(T t);
}