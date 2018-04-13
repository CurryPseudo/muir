#if UNITY_EDITOR
using SceneController;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SceneController {
    public class LevelPartsEdit : MonoBehaviour, IPath{
        [ReadOnly]
        public string levelPartsPath = "Assets/Prefabs/LevelParts/";
        string IPath.getPath() {
            return levelPartsPath;
        }
        private LevelPartProperty[] getPropertys() {
            List<LevelPartProperty> rList = new List<LevelPartProperty>();
            var os = AssetDatabase.LoadAllAssetsAtPath(levelPartsPath);
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
        [OnValueChanged("UpdateIntervals")]
        public float diffInterval = 10;
        private Intervals intervals;
        public void UpdateIntervals() {
            intervals = new Intervals(getPropertys(), diffInterval);
            intervalStrs = intervals.GetIntervalStrs();
        }
        [TabGroup("关卡片段列表")]
        [OnValueChanged("IntervalStrChanged")]
        [ValueDropdown("intervalStrs")]
        [LabelText("难度区间")]
        public string intervalStr;
        private string[] intervalStrs;
        public void IntervalStrChanged() {
            if(intervalStr != "") {
                lpChooses = intervals.GetLevelChooses(intervalStr);
            }
        }
        [TableList]
        [TabGroup("关卡片段列表")]
        [LabelText("关卡列表")]
        [ReadOnly]
        public LpChoose[] lpChooses;
        [TabGroup("关卡片段列表")]
        [Button("编辑")]
        public void EditLevelPart() {

        }
        [TabGroup("关卡片段列表")]
        [Button("复制新建")]
        public void CopyNewLevelPart() {

        }
        
        [TabGroup("新建空白关卡片段")]
        [LabelText("信息")]
        public LevelPartInfo lptp;

        [TabGroup("新建空白关卡片段")]
        [Button("新建")]
        public void NewLevelPart() {

        }
        [Serializable]
        public class LpChoose {
            
            private List<LpChoose> parent;
            public LevelPartProperty lpp;
            [OnValueChanged("ChooseChanged")]
            public bool choosed;
            public void ChooseChanged() {
                Func<bool> parentHasTrue = ()=>{
                    foreach(var choose in parent) {
                        if(choose.choosed == true) {
                            return true;
                        }
                    }
                    return false;
                };
                if(choosed == false && !parentHasTrue()) {
                    choosed = true;
                }
                if(choosed == true) {
                    foreach(var choose in parent) {
                        if(choose != this) {
                            choose.choosed = false;
                        }
                    }
                }
            }
            public LpChoose(LevelPartProperty lpp) {
                this.lpp = lpp;
            }
        }
        public class Intervals {
            
            float interval;
            string[] strs;
            List<List<LevelPartProperty>> propertyIntervalMap;
            Dictionary<string, Func<LpChoose[]>> strMap;

            public Intervals(LevelPartProperty[] propertys, float interval) {
                this.interval = interval;
                if(propertys.Length == 0) {
                    return;
                }
                List<LevelPartProperty> list = new List<LevelPartProperty>(propertys);
                List<string> intervalStrs = new List<string>();
                propertyIntervalMap = new List<List<LevelPartProperty>>();
                list.Sort((property1, property2)=>(property1.info.difficulty.CompareTo(property2.info.difficulty)));
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
                        strMap.Add(str, ()=>(CreateChooses(propertyIntervalMap[i].ToArray())));
                    }
                    propertyIntervalMap[i].Add(property);
                }
                strs = intervalStrs.ToArray();
            }
            private LpChoose[] CreateChooses(LevelPartProperty[] propertys) {
                var result = new LpChoose[propertys.Length];
                for(int i = 0; i < result.Length; i++) {
                    result[i] = new LpChoose(propertys[i]);
                }
                result[0].choosed = true;
                return result;
            }
            public string[] GetIntervalStrs() {
                return strs;
            }
            public LpChoose[] GetLevelChooses(string intervalStr) {
                return strMap[intervalStr]();
            }
        }
    }
}
#endif