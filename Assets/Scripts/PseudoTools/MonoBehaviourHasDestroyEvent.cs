using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourHasDestroyEvent : MonoBehaviour {
	#region Properties
	#endregion
	#region Inspector
    public event System.Action onDestroy;
	#endregion
	#region Monobehaviour Methods
    protected void OnDestroy() {
        onDestroy?.Invoke();
    }
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Public Method
	#endregion
}
