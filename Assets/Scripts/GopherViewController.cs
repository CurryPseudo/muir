using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementFsm))]
public class GopherViewController : MonoBehaviourHasDestroyEvent {
	#region Properties
	#endregion
	#region Inspector
		public MovementFsm movementFsm;
		public Animator view;
	#endregion
	#region Monobehaviour Methods
        void Awake() {
			MovementFsm.OnGroundStateWithEvent.Instance.enterEventMap.AddEnterEvent(movementFsm, () => {
				view.SetBool("OnGround", true);
			}, this);
			MovementFsm.OnGroundStateWithEvent.Instance.exitEventMap.AddEnterEvent(movementFsm, () => {
				view.SetBool("OnGround", false);
			}, this);
			MovementFsm.InGroundStateWithEvent.Instance.enterEventMap.AddEnterEvent(movementFsm, () => {
				view.SetBool("Digging", true);
			}, this);
			MovementFsm.InGroundStateWithEvent.Instance.exitEventMap.AddEnterEvent(movementFsm, () => {
				view.SetBool("Digging", false);
			}, this);
			MovementFsm.InGroundStateWithEvent.Instance.excuteEventMap.AddEnterEvent(movementFsm, () => {
				Vector2 direction = movementFsm.Velocity.normalized;
				float angle = Vector2.Angle(Vector2.right, direction);
				if(direction.y < 0) {
					angle = 360 - angle;
				}
				view.gameObject.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
			}, this);
			MovementFsm.InGroundStateWithEvent.Instance.exitEventMap.AddEnterEvent(movementFsm, () => {
				view.gameObject.transform.rotation = Quaternion.identity;
			}, this);
        }
		void Update() {
			view.SetFloat("ySpeed", movementFsm.Velocity.y);
			view.SetFloat("xSpeed", movementFsm.Velocity.x);
		}
	#endregion
	#region Private Methods And Fields
	#endregion	
	#region Public Method
	#endregion
}
