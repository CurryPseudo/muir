using UnityEngine;
using System.Collections.Generic;
public class GUIContentUtil {
    public static GUIContent CreateLabelContent(string content) {
        GUIContent result = new GUIContent();
        result.text = content;
        return result;
    }
    public static string[] CreateGameObjectListNames(List<GameObject> list) {
        string[] result = new string[list.Count + 1];
        result[0] = "None";
        for(int i = 1; i < list.Count + 1; i++) {
            result[i] = list[i - 1].name;
        }
        return result;
    }
    public static GameObject GetGameObjectByNameInList(List<GameObject> list, string name) {
        foreach(var gb in list) {
            if(gb.name == name) {
                return gb;
            }
        }
        return null;
    }
}