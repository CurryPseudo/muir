using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMove : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Inspector
		public SequenceSpritesWithIndex sequenceSprites; //Reference for set sprites width sum.
		public float normalWidthSum;
		public MovementFsm movement;
		public float xVelocity;
	#endregion
	#region Monobehaviour Methods
	void Update() {
		xVelocity = movement.Velocity.x * (1 - sequenceSprites.widthSum / normalWidthSum);
		transform.position += new Vector3(1, 0, 0) * xVelocity * Time.deltaTime;
	}
	#endregion
	#region Public Method
	#endregion
}
