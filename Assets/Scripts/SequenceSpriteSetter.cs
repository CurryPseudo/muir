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
            if(index == -1) {
                sequenceSpriteWithIndex.IncreaseIndex();
                index = sequenceSpriteWithIndex.NowIndex;
                Sprite targetSprite = sequenceSpriteWithIndex.spriteList[index];
                GetComponent<SpriteRenderer>().sprite = targetSprite;
                if(changeSize) {
                    puterPoint = GetComponent<PuterPoint>();
                    originScale = transform.localScale;
                    originDistance = puterPoint.endPoint.x - puterPoint.startPoint.x;
                    originCenter = (puterPoint.startPoint + puterPoint.endPoint) / 2;
                }
                if(changeSize) {
                    //Vector3 targetScale = originScale * (float)targetSprite.textureRect.width / targetWidth;
                    //transform.localScale = targetScale;
                }
                if(puterPoint != null && changeSize) {
                    float nowAspect = (float)targetSprite.texture.width / targetSprite.texture.height;
                    float nowDistance = originDistance * nowAspect / targetAspect;
                    puterPoint.startPoint = new Vector2(originCenter.x - nowDistance / 2, puterPoint.startPoint.y);
                    puterPoint.endPoint = new Vector2(originCenter.x + nowDistance / 2, puterPoint.endPoint.y);
                }
            }
        }
    }
    public float targetAspect = 0;
    public int index;
    public Vector3 originScale;
    public float originDistance;
    public Vector2 originCenter;
    public PuterPoint puterPoint;
    public bool changeSize;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        Texture2D texture = GetComponent<SpriteRenderer>().sprite.texture;
        targetAspect = (float)texture.width / texture.height;
    }
    void Update() {
    }
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Public Method
	#endregion
}
