
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
     private static DontDestroyOnLoad instance = null;
     void Awake(){
         if(instance == null){
              instance = this;
              DontDestroyOnLoad(gameObject);
         }else if(instance != this){
              Destroy(this.gameObject);
              return;
         }
     }
	#endregion
	#region Public Method
	#endregion
}
