using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToLoadMainScene : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Inspector
    
	#endregion
	#region Monobehaviour Methods
    public void LoadScene() {
        SceneManager.LoadSceneAsync("MainScene");
    }
	#endregion
	#region Public Method
	#endregion
}

