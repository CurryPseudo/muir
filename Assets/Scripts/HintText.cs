using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HintText : SingletonMonoBehaviourNow<HintText> {

	Text text;
	public string Text{
		get{
			return text.text;
		}
		set{
			text.text = value;
		}
	}
	protected override void _Awake(){
		text = GetComponent<Text>();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
