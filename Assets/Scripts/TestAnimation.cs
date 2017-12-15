using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour {

	Animator animator;
	SimpleTimer timer;
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {
		animator = GetComponent<Animator>();
		timer = new SimpleTimer(this, 3);
	}
	// Use this for initialization
	void Start () {
		timer.BeginTiming();
	}
	
	// Update is called once per frame
	void Update () {
		if(!timer.IsTiming) {
			animator.SetBool("InAir", true);
		}
	}
	public void SetInAirFalse() {
		animator.SetBool("InAir", false);
		timer.BeginTiming();
	}
}
