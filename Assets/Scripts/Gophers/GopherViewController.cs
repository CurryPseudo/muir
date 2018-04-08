using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementFsm))]
public class GopherViewController : MonoBehaviourHasDestroyEvent {
	#region Properties
	#endregion
	#region Inspector
		public MovementFsm movementFsm;
		public GopherController player;
		public Animator view;
		public Animator ghostView;
	#endregion
	#region Monobehaviour Methods
        void Awake() {
			player.AddEnterEventBeforeEnter<GopherController.SecondJumpState>(() => {
				view.SetTrigger("ReJump");
			}, this);
			movementFsm.AddEnterEventBeforeEnter<MovementFsm.OnGroundState>(() => {
				view.SetBool("OnGround", true);
			}, this);
			movementFsm.AddEnterEventBeforeExit<MovementFsm.OnGroundState>(() => {
				view.SetBool("OnGround", false);
			}, this);
			movementFsm.AddEnterEventBeforeEnter<MovementFsm.InGroundState>(() => {
				view.SetBool("Digging", true);
			}, this);
			movementFsm.AddEnterEventBeforeExit<MovementFsm.InGroundState>(() => {
				view.SetBool("Digging", false);
			}, this);
			movementFsm.AddEnterEventBeforeExcute<MovementFsm.InGroundState>(() => {
				Vector2 direction = movementFsm.Velocity.normalized;
				float angle = Vector2.Angle(Vector2.right, direction);
				if(direction.y < 0) {
					angle = 360 - angle;
				}
				view.gameObject.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
			}, this);
			movementFsm.AddEnterEventBeforeExit<MovementFsm.InGroundState>(() => {
				view.gameObject.transform.rotation = Quaternion.identity;
			}, this);
			movementFsm.AddEnterEventBeforeEnter<MovementFsm.DeadState>(() => {
				view.SetTrigger("Die");
			}, this);
			movementFsm.AddEnterEventBeforeEnter<MovementFsm.DeadState>(() => {
				ghostView.gameObject.transform.parent.SetParent(null, true);
				ghostView.SetTrigger("Die");
			}, this);
			movementFsm.AddEnterEventBeforeExcute<MovementFsm.DeadState>(() => {
				if(movementFsm.Velocity.x > 0) {
					Vector3 scale = view.transform.localScale;
					scale.x = Mathf.Abs(scale.x) * -1;
					view.transform.localScale = scale;
				}
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
