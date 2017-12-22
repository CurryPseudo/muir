using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : SingletonMonoBehaviourNow<GameStatus> {
	#region Properties
    public int RecentScore{
        get{
            return recentScore;
        }
        set{
            recentScore = value;
            if(recentScore > maxScore) {
                PlayerPrefs.SetInt("MaxScore", recentScore);
            }
        }
    }
	#endregion
	#region Private Methods And Fields
    private int recentScore = 0;
	#endregion	
	#region Inspector
    public bool deadInterface = false;
    public int maxScore;
    
	#endregion
	#region Monobehaviour Methods
    void Update() {
        if(!PlayerPrefs.HasKey("MaxScore")) {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
        maxScore = PlayerPrefs.GetInt("MaxScore");
    }
	#endregion
	#region Public Method
	#endregion
}
