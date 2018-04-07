using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementFsm : FiniteStateMachineMonobehaviour<MovementFsm> {
    #region Properties
    public Vector2 Velocity {
        get{
            return rigidbody2D.velocity;
        }
        set{
            rigidbody2D.velocity = value;
        }
    }
    public float UpdateTime{
        get{
            return Time.fixedDeltaTime;
        }
    }
    public Vector2 Position{
        get{
            return rigidbody2D.position;
        }
        set{
            rigidbody2D.position = value;
        }
    }

    public Vector2 VelocityBeforeBacktrack
    {
        get
        {
            return velocityBeforeBacktrack;
        }
    }

    public List<RaycastHit2D> DieHits
    {
        get
        {
            return dieHits;
        }

        set
        {
            dieHits = value;
        }
    }


    #endregion
    #region Inspector
    [Header("Force")]
		public float yGravityForce = 1;
		public float yUpForceInGround = 1;
		public float yDieGravityForce = 20;
		[Header("Speed")]
		public float yMaxSpeed = 5;
		public float yMaxSpeedInGroundUp = 2;
		public float yMaxSpeedInGroundDown = 2;
		public float yDigJumpSpeed = 2;
		[Header("Layer Mask")]
		public LayerMask onGroundLayer;
		public LayerMask backtrackLayer;
		public LayerMask bottomLayer;
		[Header("Other")]
		public float backtrackWidth = 0.1f;
		public Vector2 readOnlyVelocity;
		public new Rigidbody2D rigidbody2D;
		public BoxRayCaster boxRayCaster;
		public GopherController controller;
	#endregion
	
	#region Monobehaviour Methods
		protected override void FixedUpdateAfterFSMUpdate() {
			
		}
		void Awake()	
		{
			ChangeState<OnGroundStateWithEvent>();
		}
		void Update()
		{
			readOnlyVelocity = Velocity;
		}
		void Start () {

		}
		void OnDestroy() {
			if(destroyAction != null) {
				destroyAction();
			}
		}
	#endregion
	#region Private Methods And Fields
		private Vector2 velocityBeforeBacktrack;
		private List<RaycastHit2D> dieHits;
		event System.Action destroyAction;
		private void AddEnterAction(BoxRayCaster.RayTrigger trigger, System.Action<RaycastHit2D> action) {
			trigger.enterAction += action;
			destroyAction += ()=>{
				trigger.enterAction -= action;
			};
		}
		private void AddExitAction(BoxRayCaster.RayTrigger trigger, System.Action<Collider2D> action) {
			trigger.exitAction += action;
			destroyAction += ()=>{
				trigger.exitAction -= action;
			};
		}
		
		private void ProcessCollisionBacktrack(BoxRayCaster.RayTrigger trigger) {
			if(trigger.CheckCollision(backtrackLayer)) {
				velocityBeforeBacktrack = Velocity;
				Backtrack(trigger);
			}
		}
		private void Backtrack(BoxRayCaster.RayTrigger trigger) {
			Vector2 rayDirection = trigger.RayDirection; 
			BoxCollider2D box = boxRayCaster.boxCollider2D;
			Vector2 nowWorldCenter = transform.TransformPoint(box.offset);
			Vector2 rayAbsDirection = new Vector2(Mathf.Abs(rayDirection.x), Mathf.Abs(rayDirection.y));
			Vector2 lastWorldCenter = nowWorldCenter - Vector2.Scale(Velocity, rayAbsDirection) * UpdateTime;
			float step = trigger.RayStep;
			float length = 100;
			ThreeRayCast threeRayCast = new ThreeRayCast(lastWorldCenter, rayDirection, step, length, backtrackLayer);
			RaycastHit2D? result = threeRayCast.GetNearest();
			Debug.Assert(result.HasValue);
			Vector2 hitPoint = result.Value.point;
			float deltaValue = trigger.GetWorldDirectionValue(lastWorldCenter, hitPoint);
			//deltaValue += backtrackWidth;
			if(trigger.GetNeedVector2Value(Velocity) != 0) {
				//Vector2 nextWorldCenter = lastWorldCenter + deltaValue / trigger.GetNeedVector2Value(Velocity) * Velocity;
				Vector2 nextWorldCenter = lastWorldCenter + deltaValue * rayAbsDirection;
				Position += nextWorldCenter - nowWorldCenter;
				if(rayDirection.x == 0) {
					Velocity = new Vector2(Velocity.x, 0);
				}
				else {
					Velocity = new Vector2(0, Velocity.y);
				}
			}

		}
	#endregion	
	
	#region State
	
    public class OnGroundStateWithEvent : StateSingleton<OnGroundStateWithEvent>
    {
        public override void OnEnterWithEvent(MovementFsm fsm)
        {
        }
        public override void OnExcuteWithEvent(MovementFsm fsm)
        {
			
			if(!fsm.boxRayCaster.Down.CheckCollision(fsm.onGroundLayer)) {
				fsm.ChangeState<InAirStateWithEvent>();
				return;
			}
			
		}
        public override void OnExitWithEvent(MovementFsm fsm)
        {
        }
		public override string GetStateName() {
			return "OnGround";
		}
    }
	public class InAirStateWithEvent : StateSingleton<InAirStateWithEvent> 
	{
        public override void OnEnterWithEvent(MovementFsm fsm)
        {
        }

        public override void OnExcuteWithEvent(MovementFsm fsm)
        {
			fsm.Velocity = fsm.Velocity + Vector2.down * fsm.yGravityForce * fsm.UpdateTime;
			if(Mathf.Abs(fsm.Velocity.y) > fsm.yMaxSpeed) {
				fsm.Velocity = new Vector2(fsm.Velocity.x, Mathf.Sign(fsm.Velocity.y) * fsm.yMaxSpeed);
			}
			if(fsm.boxRayCaster.Down.CheckCollision(fsm.onGroundLayer)) {
				fsm.ChangeState<OnGroundStateWithEvent>();
				fsm.Backtrack(fsm.boxRayCaster.Down);
			}
			
			
        }

        public override void OnExitWithEvent(MovementFsm fsm)
        {
        }
		public override string GetStateName() {
			return "InAir";
		}
    }
	public class InGroundStateWithEvent : StateSingleton<InGroundStateWithEvent>
	{
		public override void OnEnterWithEvent(MovementFsm fsm) {
		}
		public override void OnExcuteWithEvent(MovementFsm fsm) {
			float velocityY = fsm.Velocity.y;
			velocityY += fsm.yUpForceInGround * fsm.UpdateTime;
			velocityY = Mathf.Clamp(velocityY, -fsm.yMaxSpeedInGroundDown, fsm.yMaxSpeedInGroundUp);
			fsm.Velocity = new Vector2(fsm.Velocity.x, velocityY);
			if(!fsm.boxRayCaster.Down.CheckCollision(fsm.onGroundLayer)) {
				fsm.Velocity = new Vector2(fsm.Velocity.x, fsm.yDigJumpSpeed);
				fsm.ChangeState<InAirStateWithEvent>();
			}
		}
		public override void OnExitWithEvent(MovementFsm fsm) {
		}
		public override string GetStateName() {
			return "InGround";
		}
	}
    public class DeadStateWithEvent : StateSingleton<DeadStateWithEvent>
    {

		public override void OnEnterWithEvent(MovementFsm fsm) {
			var dieHits = fsm.dieHits;
			Vector2 originVelocity = fsm.Velocity;
			Vector2 averageNormal = Vector2.zero;
			foreach(var dieHit in dieHits) {
				averageNormal += dieHit.normal.normalized;
			}
			originVelocity = Vector2.Reflect(originVelocity,averageNormal.normalized);
			fsm.Velocity = originVelocity;
		}
		public override void OnExcuteWithEvent(MovementFsm fsm) {
			fsm.Velocity += Vector2.down * fsm.yDieGravityForce * fsm.UpdateTime;
		}
        public override string GetStateName()
        {
			return "Dead";
        }
    }
	public class FrozenStateWithEvent : StateSingleton<FrozenStateWithEvent> 
	{
		public override string GetStateName() {
			return "Frozen";
		}
	}
    #endregion
}
