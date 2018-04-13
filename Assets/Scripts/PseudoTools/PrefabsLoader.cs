#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using PseudoTools;
using System.Collections.Generic;
using System.Reflection;
namespace PseudoTools {
    public class PrefabsLoader : ScriptableObject {
        public string path;
        public T[] LoadPrefabs<T>() where T : class{
            object[] os = AssetDatabase.LoadAllAssetsAtPath(path);
            List<T> result = new List<T>();
            foreach(var o in os) {
                if(o.GetType() == typeof(T)) {
                    result.Add(o as T);
                }
            }
            return result.ToArray();
        }
    }
}
#endif