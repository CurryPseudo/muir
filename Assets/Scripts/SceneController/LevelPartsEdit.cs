#if UNITY_EDITOR
using SceneController;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PseudoTools;
namespace SceneController {
    [CreateAssetMenu]
    public class LevelPartsEdit : SerializedScriptableObject, IPath{
        [ReadOnly]
        public string levelPartsPath = "Assets/Prefabs/LevelParts/";
        string IPath.getPath() {
            return levelPartsPath;
        }
        private LevelPartProperty[] getPropertys() {
            List<LevelPartProperty> rList = new List<LevelPartProperty>();
            var os = AssetsUtility.LoadFolderAssets<GameObject>(levelPartsPath);
            foreach(var o in os) {
                var lpp = (o as GameObject).GetComponent<LevelPartProperty>();
                if(lpp != null) {
                    rList.Add(lpp);
                }
            }
            return rList.ToArray();
        }
        [TabGroup("关卡片段列表")]
        [LabelText("难度区间间隔")]
        [ValidateInput("validInterval")]
        public float diffInterval = 10;
        private bool validInterval(float interval) {
            return interval > 0;
        }
        private Intervals intervals;
        [TabGroup("关卡片段列表")]
        [Button("更新难度区间列表")]
        public void UpdateIntervals() {
            if(validInterval(diffInterval)) {
                intervals = new Intervals(getPropertys(), diffInterval);
                intervalStrs = intervals.GetIntervalStrs();
                if(intervalStrs != null) {
                    intervalStr = intervalStrs[0];
                }
                else {
                    intervalStr = "无";
                    lpPreviews = new LpPreview[0];
                }
                IntervalStrChanged();
            }
        }
        [TabGroup("关卡片段列表")]
        [OnValueChanged("IntervalStrChanged")]
        [ValueDropdown("intervalStrs")]
        [LabelText("难度区间")]
        public string intervalStr = "无";
        private string[] intervalStrs;
        public void IntervalStrChanged() {
            if(intervalStr != "无") {
                lpPreviews = intervals.GetLevelChooses(intervalStr);
            }
            else {
                lpPreviews = new LpPreview[0];
            }
        }
        [TableList]
        [TabGroup("关卡片段列表")]
        [LabelText("关卡列表")]
        public LpPreview[] lpPreviews;
        [TabGroup("关卡片段列表")]
        [Button("编辑")]
        public void EditLevelPart() {

        }
        [TabGroup("关卡片段列表")]
        [Button("复制新建")]
        public void CopyNewLevelPart() {

        }
        
        [TabGroup("新建空白关卡片段")]
        [InlineProperty()]
        [LabelText("信息")]
        [HideLabel]
        public LevelPartInfo lptp;

        [TabGroup("新建空白关卡片段")]
        [Button("新建")]
        public void NewLevelPart() {
            LevelPartProperty.LevelPartsExistError(this, lptp.levelPartName);
            GameObject go = new GameObject();
            var property = go.AddComponent<LevelPartProperty>();
            property.savePath  = this;
            property.info = lptp;
            property.length = 10;
            property.Save();
        }
        [Serializable]
        public class LpPreview {
            
            [HideInInspector]
            public LpPreview[] parent;
            [ReadOnly]
            [HideInInspector]
            public LevelPartProperty lpp;
            [InlineProperty]
            [LabelText("信息")]
            public LevelPartInfo info;
            [LabelText("预览")]
            [InlineEditor(InlineEditorModes.SmallPreview)]
            [ReadOnly]
            public GameObject go;
            public LpPreview(LevelPartProperty lpp, LpPreview[] parent) {
                this.lpp = lpp;
                this.info = lpp.info;
                this.parent = parent;
                this.go = lpp.gameObject;
            }
        }
        public class Intervals {
            
            float interval;
            string[] strs;
            List<List<LevelPartProperty>> propertyIntervalMap;
            Dictionary<string, Func<LpPreview[]>> strMap;

            public Intervals(LevelPartProperty[] propertys, float interval) {
                this.interval = interval;
                if(propertys.Length == 0) {
                    return;
                }
                List<LevelPartProperty> list = new List<LevelPartProperty>(propertys);
                List<string> intervalStrs = new List<string>();
                propertyIntervalMap = new List<List<LevelPartProperty>>();
                list.Sort((property1, property2)=>(property1.info.difficulty.CompareTo(property2.info.difficulty)));
                strMap = new Dictionary<string, Func<LpPreview[]>>();
                int i = 0;
                propertyIntervalMap.Add(null);
                foreach(var property in list) {
                    while(property.info.difficulty >= (i + 1) * interval) {
                        i++;
                        propertyIntervalMap.Add(null);
                    }
                    if(propertyIntervalMap[i] == null) {
                        propertyIntervalMap[i] = new List<LevelPartProperty>();
                        string str = "[" + (i * interval).ToString() + ", " + ((i + 1)  * interval) + ")";
                        intervalStrs.Add(str);
                        strMap.Add(str, createFunc(i));
                    }
                    propertyIntervalMap[i].Add(property);
                }
                strs = intervalStrs.ToArray();
            }
            private Func<LpPreview[]> createFunc(int index) {
                return ()=>(CreateChooses(propertyIntervalMap[index].ToArray()));
            }
            private LpPreview[] CreateChooses(LevelPartProperty[] propertys) {
                var result = new LpPreview[propertys.Length];
                for(int i = 0; i < result.Length; i++) {
                    result[i] = new LpPreview(propertys[i], result);
                }
                return result;
            }
            public string[] GetIntervalStrs() {
                return strs;
            }
            public LpPreview[] GetLevelChooses(string intervalStr) {
                return strMap[intervalStr]();
            }
        }
    }
}
#endif