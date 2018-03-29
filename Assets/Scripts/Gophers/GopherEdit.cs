using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GopherEdit: MonoBehaviour {
	#region Properties
	#endregion
	#region Inspector
	public GopherController controller;
	public MovementFsm movement;
	#endregion
	#region Monobehaviour Methods
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Public Method
	public float CalculateDistance(float originSpeed, float force) {
		return originSpeed * originSpeed / (2 * force);
	}
	public float CalculateSpeed(float distance, float force) {
		return Mathf.Sqrt(2 * force * distance);
	}
	public float CaluclateForce(float originSpeed, float distance) {
		return CalculateDistance(originSpeed, distance);
	}
	#endregion
}
