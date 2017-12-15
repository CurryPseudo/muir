using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour {
	#region Inspector
		public bool lockX;
		public bool lockY;
		public bool lockZ;
	#endregion
	// Use this for initialization
	void Start () {
	}
	
	/// <summary>
	/// Callback to draw gizmos only if the object is selected.
	/// </summary>
	void OnDrawGizmosSelected()
	{
	}
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = transform.position;
		if(!lockX) {
			newPosition.x = Camera.main.transform.position.x;
		}
		if(!lockY) {
			newPosition.y = Camera.main.transform.position.y;
		}
		if(!lockZ) {
		newPosition.z = Camera.main.transform.position.z;
		}
		Camera.main.gameObject.transform.position = newPosition; 
	}
}
