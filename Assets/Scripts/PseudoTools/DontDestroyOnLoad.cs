
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Inspector
	#endregion
	#region Monobehaviour Methods
     void Awake(){
        DontDestroyOnLoad(gameObject);
     }
	#endregion
	#region Public Method
	#endregion
}
