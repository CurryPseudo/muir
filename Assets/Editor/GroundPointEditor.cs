using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PuterPoint))]
public class GroundPointEditor : Editor {
	public void OnSceneGUI()
	{
		PuterPoint script = (PuterPoint)target;

		Handles.color = Color.yellow;
		script.StartPoint = Handles.PositionHandle(script.StartPoint, Quaternion.identity);
		Handles.ArrowHandleCap(0, script.StartPoint, Quaternion.identity, 1f, EventType.Repaint);
		Handles.color = Color.red;
		script.EndPoint = Handles.PositionHandle(script.EndPoint, Quaternion.identity);
		Handles.ArrowHandleCap(1, script.EndPoint, Quaternion.identity, 1f, EventType.Repaint);
	}

}
