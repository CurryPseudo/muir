using PseudoTools;
using UnityEngine.SceneManagement;
using UnityEngine;
[ReceiveEvent("PlayerDie")]
public class LoadDeadSceneByPlayerDie : ObserverMonoBehaviour {
    public float afterTime = 2;
    public void ReceivePlayerDie() {
        Timer.BeginATimer(afterTime , ()=>{
			if(GameStatus.Now != null) {
				GameStatus.Now.deadInterface = true;
				SceneManager.LoadSceneAsync("Home");
			}else {
				SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
			}
        }, this);
    }
}