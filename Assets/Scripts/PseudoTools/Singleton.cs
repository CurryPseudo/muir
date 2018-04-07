public class Singleton<T> where T : Singleton<T>, new(){

    static T instance = null;
    public static T Instance{
        get{
            if(instance == null) {
                instance = new T();
            }
            return instance;
        }
    }
}