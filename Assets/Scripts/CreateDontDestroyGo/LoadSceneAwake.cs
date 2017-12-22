
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAwake : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Inspector
    public string sceneName;
    public bool async;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        if(async) {
            SceneManager.LoadSceneAsync(sceneName);
        }
        else {
            SceneManager.LoadScene(sceneName);
        }
    }
	#endregion
	#region Public Method
	#endregion
}
