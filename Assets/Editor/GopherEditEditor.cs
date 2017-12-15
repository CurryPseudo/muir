using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GopherEdit))]
public class GopherEditEditor : Editor {
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		GopherEdit script = (GopherEdit)target;
		float jumpHeight = EditorGUILayout.FloatField("普通跳跃高度", script.CalculateDistance(script.controller.yJumpSpeed, script.movement.yGravityForce));
		float highJumpHeight = EditorGUILayout.FloatField("出土跳跃高度", script.CalculateDistance(script.movement.yDigJumpSpeed, script.movement.yGravityForce));
		float yForce = EditorGUILayout.FloatField("重力加速度（维持各跳跃高度）", script.movement.yGravityForce);
		script.controller.yJumpSpeed = script.CalculateSpeed(jumpHeight, yForce);
		script.movement.yDigJumpSpeed = script.CalculateSpeed(highJumpHeight, yForce);
		script.movement.yGravityForce = yForce;
	}
	public void OnSceneGUI() {
		GopherEdit script = (GopherEdit)target;
		float jumpHeight = script.CalculateDistance(script.controller.yJumpSpeed, script.movement.yGravityForce);
		float highJumpHeight = script.CalculateDistance(script.movement.yDigJumpSpeed, script.movement.yGravityForce); 
		Vector3 worldPosition = script.transform.position;
		Handles.color = Color.white;
		Handles.RectangleHandleCap(0, worldPosition, Quaternion.identity, 0.07f, EventType.Repaint);
		Handles.RectangleHandleCap(1, worldPosition + Vector3.up * jumpHeight, Quaternion.identity, 0.07f, EventType.Repaint);
		Handles.RectangleHandleCap(2, worldPosition + Vector3.up * highJumpHeight, Quaternion.identity, 0.07f, EventType.Repaint);
	}

}
