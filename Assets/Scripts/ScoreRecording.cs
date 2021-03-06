using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecording : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Inspector
    public int score;
    private float originX;
    public float scoreStep;
    public bool pause = false;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        originX = transform.position.x;
    }
    void Update()
    {
        if(!pause) {
            float floatScore = (transform.position.x - originX) / scoreStep;
            score = (int)floatScore;
        }
    }
	#endregion
	#region Public Method
	#endregion
}

