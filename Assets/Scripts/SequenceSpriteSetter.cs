using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SequenceSpriteSetter : MonoBehaviour {
	#region Properties
	#endregion
	#region Inspector
    private SequenceSpritesWithIndex sequenceSpriteWithIndex;
    public SequenceSpritesWithIndex SequenceSpritesWithIndex{
        set{
            sequenceSpriteWithIndex = value;
            sequenceSpriteWithIndex.IncreaseIndex();
            if(index == -1) {
                index = sequenceSpriteWithIndex.nowIndex;
            }
        }
    }
    public int index;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
    }
    void Update() {
        if(sequenceSpriteWithIndex != null && index != -1) {
            GetComponent<SpriteRenderer>().sprite = sequenceSpriteWithIndex.spriteList[index];
        }
    }
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Public Method
	#endregion
}
