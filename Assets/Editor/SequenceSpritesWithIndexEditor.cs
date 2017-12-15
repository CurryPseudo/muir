using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SequenceSpritesWithIndex))]
public class SequenceSpritesWithIndexiEditor : Editor {
    public SequenceSpritesWithIndex monoBehaviour;
    public override void OnInspectorGUI() {
        monoBehaviour = target as SequenceSpritesWithIndex;

        DrawDefaultInspector();
        SequenceSprites sequenceSprites = EditorGUILayout.ObjectField("SequenceSprites", null as Object, typeof(SequenceSprites), true) as SequenceSprites;
        if(sequenceSprites != null) {
            monoBehaviour.SetSpriteList(sequenceSprites);
        }
    }
}