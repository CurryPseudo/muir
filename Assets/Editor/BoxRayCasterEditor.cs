using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(BoxRayCaster))]
public class BoxRayCasterEditor : Editor {
	BoxRayCaster monoBehaviour;
	public override void OnInspectorGUI() {
		monoBehaviour = target as BoxRayCaster;
		DrawBoxTriggerStatus("Up", monoBehaviour.Up);
		DrawBoxTriggerStatus("Down", monoBehaviour.Down);
		DrawBoxTriggerStatus("Left", monoBehaviour.Left);
		DrawBoxTriggerStatus("Right", monoBehaviour.Right);
		DrawDefaultInspector();
	}
	public void DrawBoxTriggerStatus(string name, BoxRayCaster.RayTrigger trigger) {
		if(trigger != null) {
			EditorGUILayout.LabelField(name);
			EditorGUILayout.TextArea(trigger.CollisionStatus);
		}
	}
}
