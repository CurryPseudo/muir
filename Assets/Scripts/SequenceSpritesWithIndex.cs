using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceSpritesWithIndex : MonoBehaviour {
	#region Properties
	public List<Sprite> spriteList = new List<Sprite>();
	public int nowIndex = 0;
	public static int globalIndex = 0;
	public float widthSum; //Sequence sprites' width sum;
	#endregion
	#region Inspector
	void Update() {
	}
	#endregion
	#region Monobehaviour Methods
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Public Method
	public void SetSpriteList(SequenceSprites sequenceSprites) {
		spriteList = new List<Sprite>(sequenceSprites.prefabs);
		widthSum = 0;
		foreach(var sprite in spriteList) {
			widthSum += sprite.textureRect.width;
		}
	}
	public void IncreaseIndex() {
		nowIndex++;
		if(nowIndex >= spriteList.Count) {
			nowIndex = 0;
		}
	}
	#endregion
}
