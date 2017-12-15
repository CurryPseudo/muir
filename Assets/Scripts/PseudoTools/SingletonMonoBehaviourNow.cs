using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class SingletonMonoBehaviourNow<T> : MonoBehaviourHasAwake where T : SingletonMonoBehaviourNow<T>{
    static T now = null;
    public static T Now{
        get{
            return now;
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected sealed override void Awake()
    {
        now = this as T;
        _Awake();
    }
    protected virtual void _Awake() {}

}
public class MonoBehaviourHasAwake : MonoBehaviour{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        
    }
}