using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SequenceSprites))]
public class SequencePrefabsEditor : Editor {
    public SequenceSprites monoBehaviour;
    public override void OnInspectorGUI() {
        monoBehaviour = target as SequenceSprites;
        DrawDefaultInspector();
        GUIContent content = new GUIContent();
        content.text = "Load";

        if(GUILayout.Button(content)) {
            monoBehaviour.Read();
        }
    }
}