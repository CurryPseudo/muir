#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections;
using PseudoTools;
using UnityEngine;
using UnityEditor;
namespace PseudoTools {
    public class AssetsUtility {
        public static T[] LoadFolderAssets<T>(string path) where T : UnityEngine.Object {
            ArrayList al = new ArrayList();
            Func<string,string,string> removeHead = (str, head) => {
                if(str.IndexOf(head) == 0) {
                    str = str.Substring(head.Length);
                }
                return str;
            };
            path = removeHead(path, "Assets/");
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
    
            foreach (string fileName in fileEntries)
            {
                int assetPathIndex = fileName.IndexOf("Assets");
                string localPath = fileName.Substring(assetPathIndex);
    
                T t = AssetDatabase.LoadAssetAtPath<T>(localPath);
    
                if (t != null)
                    al.Add(t);
            }
            T[] result = new T[al.Count];
            for (int i = 0; i < al.Count; i++)
                result[i] = (T)al[i];
    
            return result;
        } 
    }
}
#endif