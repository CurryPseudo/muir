using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PuterEdit))]
public class PuterEditEditor : Editor {
	PuterEdit monoBehaviour;
	GameObjectPuter puter;
	bool editorFoldOut = true;
	string newPrefabPath = "";
	public override void OnInspectorGUI() {
		monoBehaviour = target as PuterEdit;
		puter = monoBehaviour.puter;
		DrawDefaultInspector();
		editorFoldOut = EditorGUILayout.Foldout(editorFoldOut, "给桉宝的编辑器！");
		if(editorFoldOut) {
			EditorGUI.indentLevel = 1;
			GameObject prefab = null;
			if(monoBehaviour.nowEditInstance != null) {
				prefab = monoBehaviour.nowEditInstance.GetComponent<PrefabParentSetter>().prefabParent;
			}
			EditorGUILayout.LabelField("这里可以选择编辑的障碍物（注意：在play时必须有一个正在编辑的障碍物）");
			GameObject chooseGameObject = PopUpGameObject(prefab);
			SetNowEditGameObject(chooseGameObject);
			EditorGUILayout.LabelField("这里可以选择新建障碍物的模板");
			EditorGUI.indentLevel = 1;
			monoBehaviour.newTemplate = PopUpGameObject(monoBehaviour.newTemplate);
			EditorGUILayout.LabelField("新建或者重命名的障碍物名");
			newPrefabPath = EditorGUILayout.TextField(newPrefabPath).Trim();
			string fullPath = "Assets/Prefabs/" + newPrefabPath + ".prefab";
			if(GUILayout.Button("新建一个障碍物")) {
				if(newPrefabPath.Trim() == "") {
					EditorUtility.DisplayDialog("警告", "名字为空", "好吧……");
				}
				else if(monoBehaviour.newTemplate == null) {
					EditorUtility.DisplayDialog("警告", "新建的模板障碍物不能为None", "好吧……");
				}
				else {
					GameObject loadPrefab = AssetDatabase.LoadAssetAtPath(fullPath, typeof(GameObject)) as GameObject;
					if(loadPrefab == null || (loadPrefab != null && EditorUtility.DisplayDialog("警告", "已经存在同名障碍物，要替换它吗？", "好啊", "别啊"))) {
						GameObject newPrefab = PrefabUtility.CreatePrefab(fullPath, monoBehaviour.newTemplate);
						puter.putPrefabs.Add(newPrefab);
						SetNowEditGameObject(newPrefab);
					}
				}
			}
			EditorGUI.indentLevel = 0;
			if(GUILayout.Button("找到正在编辑的障碍物")) {
				Selection.activeGameObject = monoBehaviour.nowEditInstance;
			}
			if(GUILayout.Button("保存当前更改")) {
				GameObject nowPrefab = monoBehaviour.nowEditInstance.GetComponent<PrefabParentSetter>().prefabParent;
				puter.putPrefabs.Remove(nowPrefab);
				DestroyImmediate(monoBehaviour.nowEditInstance.GetComponent<PrefabParentSetter>());
				nowPrefab = PrefabUtility.ReplacePrefab(monoBehaviour.nowEditInstance, nowPrefab, ReplacePrefabOptions.ConnectToPrefab);
				puter.putPrefabs.Add(nowPrefab);
				monoBehaviour.nowEditInstance.AddComponent<PrefabParentSetter>().prefabParent = nowPrefab;
				PrefabUtility.DisconnectPrefabInstance(monoBehaviour.nowEditInstance);
				EditorUtility.DisplayDialog("Ojbk", "妥妥地保存了", "行");
			}
			if(GUILayout.Button("删除当前编辑障碍物")) {
				if(monoBehaviour.nowEditInstance == null) {
					EditorUtility.DisplayDialog("警告", "正在编辑的障碍物不能为None", "好吧……");
				}
				else {
					if(EditorUtility.DisplayDialog("喂", "确定真的要删除这个障碍物吗，这个操作没法反悔！", "好啊", "等等")) {
						GameObject removeOne = monoBehaviour.nowEditInstance;
						GameObject parentPrefab = removeOne.GetComponent<PrefabParentSetter>().prefabParent;
						string path = "Assets/Prefabs/" + parentPrefab.name + ".prefab";
						puter.putPrefabs.Remove(parentPrefab);
						AssetDatabase.DeleteAsset(path);
						SetNowEditGameObjectNull();
					}
				}
			}
			if(GUILayout.Button("重命名当前障碍物")) {
				if(newPrefabPath == "") {
					EditorUtility.DisplayDialog("警告", "名字为空", "好吧……");
				}
				else if(monoBehaviour.nowEditInstance == null) {
					EditorUtility.DisplayDialog("警告", "正在编辑的障碍物不能为None", "好吧……");
				}
				else {
					GameObject existedGo = AssetDatabase.LoadAssetAtPath(fullPath, typeof(GameObject)) as GameObject;
					if(existedGo == null) {
						GameObject parentPrefab = monoBehaviour.nowEditInstance.GetComponent<PrefabParentSetter>().prefabParent;
						string originPath = AssetDatabase.GetAssetPath(parentPrefab);
						parentPrefab.name = newPrefabPath;
						AssetDatabase.RenameAsset(originPath, newPrefabPath);
					}
					else {
						EditorUtility.DisplayDialog("警告", "重命名文件已存在", "好吧……");
					}
				}
			}
			EditorGUI.indentLevel = 0;
		}
	}
	public GameObject PopUpGameObject(GameObject prefab) {
		int lastIndex = prefab != null ? puter.putPrefabs.IndexOf(prefab) + 1 : 0;
		int index = EditorGUILayout.Popup(lastIndex, GUIContentUtil.CreateGameObjectListNames(puter.putPrefabs));
		if(index == 0) {
			return null;
		}
		else {
			return puter.putPrefabs[index - 1];
		}
	}
	public void SetNowEditGameObject(GameObject chooseGameObject) {
		if(monoBehaviour.nowEditInstance == null || chooseGameObject != monoBehaviour.nowEditInstance.GetComponent<PrefabParentSetter>().prefabParent) {
			if(monoBehaviour.nowEditInstance != null) {
				puter.nowInstances.Remove(monoBehaviour.nowEditInstance);
				DestroyImmediate(monoBehaviour.nowEditInstance);
			}
			if(chooseGameObject != null) {
				GameObject newPrefab = chooseGameObject;
				monoBehaviour.nowEditInstance = Instantiate(newPrefab);
				//monoBehaviour.nowEditInstance = PrefabUtility.InstantiatePrefab(newPrefab) as GameObject;
				monoBehaviour.nowEditInstance.transform.SetParent(monoBehaviour.editParentTransform);
				monoBehaviour.nowEditInstance.AddComponent<PrefabParentSetter>().prefabParent = newPrefab;
				puter.nowInstances.Add(monoBehaviour.nowEditInstance);
				puter.newestInstance = monoBehaviour.nowEditInstance;
			}else {
				monoBehaviour.nowEditInstance = null;
			}
		}
	}
	public void SetNowEditGameObjectNull() {
			puter.nowInstances.Remove(monoBehaviour.nowEditInstance);
			DestroyImmediate(monoBehaviour.nowEditInstance);
			monoBehaviour.nowEditInstance = null;
	}
}
