using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SimpleTimer{
    MonoBehaviour mb;
    float stepTime;
    bool isTiming = false;
    Coroutine coroutine;
    public bool IsTiming{
        get{
            return isTiming;
        }
    }
    public void BeginTiming() {
        if(!isTiming) {
            isTiming = true;
            coroutine = Timer.BeginATimer(stepTime, ()=>{isTiming = false;}, mb);
        }
    }
    public void StopTiming() {
        if(isTiming) {
            isTiming = true;
            mb.StopCoroutine(coroutine);
        }
    }
    public SimpleTimer(MonoBehaviour _mb, float _stepTime) {
        mb = _mb;
        stepTime = _stepTime;
    }
}