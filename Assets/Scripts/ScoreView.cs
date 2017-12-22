using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Inspector
    public Text scoreText;
    public string scoreHead;
    public ScoreRecording score;
	#endregion
	#region Monobehaviour Methods
    void Update() {
        scoreText.text = scoreHead + score.score.ToString();
    }
	#endregion
	#region Public Method
	#endregion
}

