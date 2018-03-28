using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SequenceSprite{

public class SequenceSpriteGenerator : MonoBehaviour {
	public string spritesFileDir;
	public string spritesFileSuffix = ".png";
	public DirectionSign xDirection;
	public DirectionSign yDirection;
	[ReadOnly]
	public List<GameObject> goList = new List<GameObject>();
	#if UNITY_EDITOR
	[ButtonGroup("序列精灵")]
	[Button("生成序列精灵")]
	private void BuildSprite() {
		ClearSprite();
		int index = 1;
		if(spritesFileDir[spritesFileDir.Length - 1] != '/') {
			spritesFileDir += "/";
		}
		Func<string> getPath = ()=>{return spritesFileDir + index.ToString() + spritesFileSuffix;};
		Func<Sprite> getSprite = ()=>{return AssetDatabase.LoadAssetAtPath<Sprite>(getPath());};
		Func<Sprite, GameObject> createGo = (Sprite _s)=>{
			GameObject go = new GameObject(getPath());
			SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
			renderer.sprite = _s;
			go.transform.SetParent(transform, false);
			goList.Add(go);
			RelateGoList rgl = go.AddComponent<RelateGoList>();
			rgl.relateList = goList;
			return go;
		};
		GameObject lastGo = null;
		Action<GameObject> setGoPosition = (GameObject go)=>{
			if(lastGo == null) {
				lastGo = go;
				return;
			}
			Vector2 lastPositon = lastGo.transform.position;		
			Func<GameObject, Vector2> getHalfSize = (GameObject _go)=>{
				var renderer = _go.GetComponent<SpriteRenderer>();
				var size = renderer.bounds.size;
				return new Vector2(size.x / 2, size.y / 2);
			};
			Vector2 offsetSize = getHalfSize(lastGo) + getHalfSize(go);
			Vector2 offset = new Vector2(offsetSize.x * (int)xDirection, offsetSize.y * (int)yDirection);
			Vector3 offsetV3 = new Vector3(offset.x, offset.y, 0);
			go.transform.position = lastGo.transform.position + offsetV3;
			lastGo = go;
		};
		Sprite s = getSprite();
		while(s != null) {
			var go = createGo(s);
			setGoPosition(go);
			index++;
			s = getSprite();
		}
	}
	#endif
	[ButtonGroup("序列精灵")]
	[Button("清空")]
	private void ClearSprite() {
		
		while(goList.Count > 0) {
			var go = goList[0];
			go.GetComponent<RelateGoList>().removeWhenDestroyed = false;
			DestroyImmediate(go);
			goList.Remove(go);
		}
	}
	[ButtonGroup("序列精灵")]
	[Button("选中所有")]
	private void selectAll() {
		UnityEngine.Object[] selectObjects = new UnityEngine.Object[goList.Count];
		for(int i = 0; i < goList.Count; i++) {
		    selectObjects[i] = goList[i];
		}
		Selection.objects = selectObjects;
	}
}
}
