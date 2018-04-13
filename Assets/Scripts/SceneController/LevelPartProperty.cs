#if UNITY_EDITOR
using SceneController;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using System;
namespace SceneController {
    public class LevelPartProperty : SerializedMonoBehaviour{
        private GameObject relatePrefab = null;
        [ReadOnly]
        public IPath savePath;
        [LabelText("信息")]
        public LevelPartInfo info;
        [LabelText("长度")]
        public float length;
        public void setSavePath(IPath savePath) {
            this.savePath = savePath;
        }
        [InfoBox("没有已有关卡片段，保存将会新建一个片段。", "PrefabNotExist")]
        [InfoBox("有已存在关卡，保存将会更新", "PrefabExist")]
        [Button("保存")]
        public void Save() {
            if(savePath == null) {
                throw new Exception("Save path cant be null");
            }
            var finalPath = savePath.getPath() + info.levelPartName + ".prefab";
            if(AssetDatabase.LoadAssetAtPath<GameObject>(finalPath)) {
                throw new Exception("已存在同名的其他关卡片段");
            }
            var tempPath = savePath.getPath() + info + "Temp233.prefab";
            var newPrefab = PrefabUtility.CreatePrefab(tempPath, gameObject);
            if(PrefabExist()) {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(relatePrefab));
            }
            AssetDatabase.RenameAsset(tempPath, finalPath);
            relatePrefab = newPrefab;
        }
        public bool PrefabExist() {
            return relatePrefab != null;
        }
        public bool PrefabNotExist() {
            return relatePrefab == null;
        }
    }
}
[System.Serializable]
public class LevelPartInfo {
    [LabelText("关卡片段名")]   
    public string levelPartName;
    [LabelText("难度")]
    public float difficulty;
}
#endif