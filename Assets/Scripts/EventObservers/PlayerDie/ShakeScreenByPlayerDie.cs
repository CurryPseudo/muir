
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PseudoTools;


[ReceiveEvent("PlayerDie")]
public class ShakeScreenByPlayerDie : ObserverMonoBehaviour{
	#region Properties
	#endregion
	#region Private Methods And Fields
    private void ReceivePlayerDie() {
        shake.shake = true;
        Timer.BeginATimer(shakeTime, ()=> {shake.shake = false;}, this);
    }
	#endregion	
	#region Inspector
    public float shakeTime = 1.5f;
    public ScreenShake shake;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        shake = GetComponent<ScreenShake>();
    }
	#endregion
	#region Public Method
	#endregion
}
