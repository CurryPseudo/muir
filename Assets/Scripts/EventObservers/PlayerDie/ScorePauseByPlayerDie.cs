using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PseudoTools;

[ReceiveEvent("PlayerDie")]
public class ScorePauseByPlayerDie : ObserverMonoBehaviour{
	#region Properties
	#endregion
	#region Private Methods And Fields
    private void ReceivePlayerDie() {
        if(GameStatus.Now != null) {
            GameStatus.Now.RecentScore = score.score;
        }
        
        score.pause = true;
        
    }
	#endregion	
	#region Inspector
    public ScoreRecording score;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        score = GetComponent<ScoreRecording>();
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
