using UnityEditor;
using SceneController;
using UnityEngine;
[CustomEditor(typeof(PutableGo))]
public class PutableGoEditor : Editor {
    public void OnSceneGUI() {
        PutableGo pg = (PutableGo)target;

		Handles.color = Color.yellow;
		pg.NegativePointWorld = Handles.PositionHandle(pg.NegativePointWorld, Quaternion.identity);
		Handles.ArrowHandleCap(0, pg.NegativePointWorld, Quaternion.identity, HandleUtility.GetHandleSize(pg.NegativePointWorld), EventType.Repaint);
		Handles.color = Color.red;
		pg.PositivePointWorld = Handles.PositionHandle(pg.PositivePointWorld, Quaternion.identity);
		Handles.ArrowHandleCap(1, pg.PositivePointWorld, Quaternion.identity, HandleUtility.GetHandleSize(pg.PositivePointWorld), eventType: EventType.Repaint);
    }
}
