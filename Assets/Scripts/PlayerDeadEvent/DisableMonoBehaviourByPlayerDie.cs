using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMonoBehaviourByPlayerDie : MonoBehaviourHasDestroyEvent {
	#region Properties
	#endregion
	#region Inspector
		public MonoBehaviour disableMonoBehaviour;
		public MovementFsm movementFsm;
	#endregion
	#region Monobehaviour Methods
		void Start() {
			movementFsm.AddEnterEventBeforeEnter<MovementFsm.DeadState>(()=> {disableMonoBehaviour.enabled = false;}, this);
		}
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Public Method
	#endregion
}
