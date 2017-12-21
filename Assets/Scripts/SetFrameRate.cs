using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFrameRate : MonoBehaviour {

	public int frameRate = 60;
	// Use this for initialization
	void Start () {
		Application.targetFrameRate = frameRate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
