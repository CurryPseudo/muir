using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : CouldDisableEditorUpdateMonoBehaviour{
	#region private fields
		private Vector3 cameraPosition;
	#endregion
	#region Inspector
		public bool lockX;
		public bool lockY;
		public bool lockZ;
		public delegate Vector3 cameraPositionEffect(Vector3 originPos);
		public event cameraPositionEffect effect;
	#endregion
	// Use this for initialization
	void Start () {
		cameraPosition = Camera.main.transform.position;
	}
	
	/// <summary>
	/// Callback to draw gizmos only if the object is selected.
	/// </summary>
	void OnDrawGizmosSelected()
	{
	}
	// Update is called once per frame
	public override void _Update () {
		Vector3 newPosition = transform.position;
		if(!lockX) {
			newPosition.x = cameraPosition.x;
		}
		if(!lockY) {
			newPosition.y = cameraPosition.y;
		}
		if(!lockZ) {
			newPosition.z = cameraPosition.z;
		}
		cameraPosition = newPosition;
		Vector3? afterPosition = null;
		if(effect != null) {
			afterPosition = effect.Invoke(newPosition);
		}
		if(afterPosition.HasValue) {
			newPosition = afterPosition.Value;
		}
		Camera.main.gameObject.transform.position = newPosition; 
	}
	void FixedUpdate() {

	}
}
