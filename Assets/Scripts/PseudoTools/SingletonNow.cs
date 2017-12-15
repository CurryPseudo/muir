public class SingletonNow<T> where T : SingletonNow<T>{

    static T now = null;
    public static T Now{
        get{
            return now;
        }
    }
    public SingletonNow() {
        now = this as T;
    }
}