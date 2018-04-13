using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PseudoTools;

[ReceiveEvent("PlayerDie")]
public class DisableMonoBehaviourByPlayerDie : ObserverMonoBehaviour{
	#region Properties
	#endregion
	#region Inspector
		public MonoBehaviour disableMonoBehaviour;
	#endregion
	#region Monobehaviour Methods
		void Start() {
		}
	#endregion
	#region Private Methods And Fields
		private void ReceivePlayerDie() {
			disableMonoBehaviour.enabled = false;
		}

    public override void _OnEnable()
    {
    }

    public override void _OnDisable()
    {
    }
    #endregion
    #region Public Method
    #endregion
}
