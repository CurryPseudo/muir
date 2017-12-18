
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreenByPlayerDie : MonoBehaviourHasDestroyEvent {
	#region Properties
	#endregion
	#region Private Methods And Fields
    private void ShakeScreen() {
        follow.lockX = false;
        shake.shake = true;
        Timer.BeginATimer(shakeTime, ()=> {shake.shake = false;}, this);
    }
	#endregion	
	#region Inspector
    public float shakeTime = 1.5f;
    public ScreenShake shake;
    public MovementFsm player;
    public CameraFollow follow;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        MovementFsm.DeadStateWithEvent.Instance.enterEventMap.AddEnterEvent(player, ShakeScreen, this);
    }
	#endregion
	#region Public Method
	#endregion
}
