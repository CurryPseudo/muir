using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Timer{
    public static IEnumerator TimerCoroutine(float time, System.Action afterTimerAction) {
        float timeCount = 0;
        while(timeCount < time) {
            yield return null;
            timeCount += Time.deltaTime;
        }
        if(afterTimerAction != null) {
            afterTimerAction();
        }
        yield break;
    }
    public static Coroutine BeginATimer(float time, System.Action afterTimerAction, MonoBehaviour mb) {
        return mb.StartCoroutine(TimerCoroutine(time, afterTimerAction));
    }
}