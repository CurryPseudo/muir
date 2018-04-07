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
		public Animator ghostView;
	#endregion
	#region Monobehaviour Methods
        void Awake() {
			movementFsm.AddEnterEvent<MovementFsm.OnGroundStateWithEvent>(() => {
				view.SetBool("OnGround", true);
			}, swe => swe.enterEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.OnGroundStateWithEvent>(() => {
				view.SetBool("OnGround", false);
			}, swe => swe.exitEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.InGroundStateWithEvent>(() => {
				view.SetBool("Digging", true);
			}, swe => swe.enterEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.InGroundStateWithEvent>(() => {
				view.SetBool("Digging", false);
			}, swe => swe.exitEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.InGroundStateWithEvent>(() => {
				Vector2 direction = movementFsm.Velocity.normalized;
				float angle = Vector2.Angle(Vector2.right, direction);
				if(direction.y < 0) {
					angle = 360 - angle;
				}
				view.gameObject.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
			}, swe => swe.excuteEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.InGroundStateWithEvent>(() => {
				view.gameObject.transform.rotation = Quaternion.identity;
			}, swe => swe.exitEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.DeadStateWithEvent>(() => {
				view.SetTrigger("Die");
			}, swe => swe.enterEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.DeadStateWithEvent>(() => {
				ghostView.gameObject.transform.parent.SetParent(null, true);
				ghostView.SetTrigger("Die");
			}, swe => swe.enterEventMap, this);
			movementFsm.AddEnterEvent<MovementFsm.DeadStateWithEvent>(() => {
				if(movementFsm.Velocity.x > 0) {
					Vector3 scale = view.transform.localScale;
					scale.x = Mathf.Abs(scale.x) * -1;
					view.transform.localScale = scale;
				}
			}, swe => swe.excuteEventMap, this);
			
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
