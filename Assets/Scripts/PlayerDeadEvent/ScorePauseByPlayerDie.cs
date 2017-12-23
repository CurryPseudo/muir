
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePauseByPlayerDie : MonoBehaviourHasDestroyEvent {
	#region Properties
	#endregion
	#region Private Methods And Fields
    void PauseScore() {
        if(GameStatus.Now != null) {
            GameStatus.Now.RecentScore = score.score;
        }
        score.pause = true;
    }
	#endregion	
	#region Inspector
    public MovementFsm player;
    public ScoreRecording score;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        MovementFsm.DeadStateWithEvent.Instance.enterEventMap.AddEnterEvent(player, PauseScore, this);
    }
	#endregion
	#region Public Method
	#endregion
}
