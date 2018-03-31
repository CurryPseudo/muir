using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PseudoTools;

[ReceiveEvent("ScoreOver")]
public class TestEventBus : ObserverMonoBehaviour{
    public override void _OnDisable()
    {
    }

    public override void _OnEnable()
    {
    }
	public void ReceiveScoreOver (int score) {
		Debug.Log("What up" + score.ToString());
	}
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
