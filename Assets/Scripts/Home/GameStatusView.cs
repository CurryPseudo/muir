using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusView : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Inspector
    public GameStatus status;
    public Text scoreTextStart;
    public Text scoreTextDead;
	public Image buttonImage;
	public Sprite startSprite;
	public Sprite deadSprite;
	public string maxScoreHead;
	public string lastScoreHead;
	public string separateText;
	#endregion
	#region Monobehaviour Methods
	void Awake() {
		status = GameStatus.Now;
	}
	void Update() {
		scoreTextDead.text = GetMaxAndLastText();
		scoreTextStart.text = GetMaxText();
		scoreTextDead.gameObject.SetActive(status.deadInterface);
		scoreTextStart.gameObject.SetActive(!status.deadInterface);
		buttonImage.sprite = status.deadInterface ? deadSprite : startSprite;
	}
	#endregion
	#region Public Method
	public int GetMaxScore() {
		return status.maxScore;
	}
	public int GetLastScore() {
		return status.RecentScore;
	}
	public string GetMaxAndLastText() {
		return maxScoreHead + GetMaxScore().ToString() + separateText + lastScoreHead + GetLastScore().ToString();
	}
	public string GetMaxText() {
		return maxScoreHead + GetMaxScore().ToString();
	}
	#endregion
}
