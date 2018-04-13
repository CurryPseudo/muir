using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PseudoTools;
public class GopherController : FiniteStateMachineMonobehaviour<GopherController>{

	#region private fields
	#endregion
	#region Inspector
	[Header("Force")]
	public float yDiggingForce = 1;
	[Header("Speed")]
	public float xSpeed = 3;
	public float yJumpSpeed = 4;
	[Header("First Jump")]
	public float firstJumpSpeed = 2;
	public float firstJumpMaxHeight = 4;
	[Header("Second Jump")]
	public float secondJumpSpeed = 1.3f;
	public float secondJumpMaxHeight = 3;
	[Header("Jump Ultility")]
	public float maxSumHeight = 5;
	[Header("Layer Mask")]
	public LayerMask deadLayers;
	public LayerMask bottomLayers;
	[Header("Other")]
	public float holdMouseTime = 0.3f;
	public MovementFsm movementFsm;
	public BoxRayCaster onGroundBoxRayCaster;
	public BoxRayCaster diggingBoxRayCaster;
	#endregion
	#region MonoBehaviour Methods
		void Awake()
		{
			ChangeState<RunningState>(null);
			movementFsm.AddEnterEventBeforeEnter<MovementFsm.OnGroundState>(()=>ChangeState<RunningState>(null));
		}
		void Start () {
		}
		protected override void FixedUpdateAfterFSMUpdate() {
			
		}
		void OnDestroy() {
			movementFsm.RemoveEnterEventBeforeEnter<MovementFsm.OnGroundState>(()=>ChangeState<RunningState>(null));
		}
		void OnDestroyWithEvent() {
		}
	#endregion
	#region Private Methods
		/* 
		private void HitGroundAndDie(List<BoxRayCaster.RayTrigger> hitTrigger) {
			if(hitTrigger.Count == 0) return;
			movementFsm.DieTriggers = hitTrigger;
			movementFsm.ChangeStateSingleton<MovementFsm.DeadStateWithEvent>();
			Timer.BeginATimer(2, () => {SceneManager.LoadScene(0);}, this);
			ChangeStateSingleton<DeadState>();
		}
		*/
		private bool HitGroundAndDie(IEnumerable<RaycastHit2D> hits) {
			bool hasHit = false;
			foreach(var hit in hits) {
				hasHit = true;
				break;
			}
			if(!hasHit) {
				return false;
			}
			movementFsm.DieHits = new List<RaycastHit2D>(hits);
			movementFsm.ChangeState<MovementFsm.DeadState>();
			//Timer.BeginATimer(2, LoadDeadScene, this);
			ChangeState<DeadState>();
			return true;
		}
	#endregion
    #region States
    public class RunningState : StateNormal<RunningState>
    {
		public bool upJumpButton = false;
		public override void OnEnterWithEvent(GopherController fsm){
			fsm.movementFsm.Velocity = new Vector2(fsm.xSpeed, fsm.movementFsm.Velocity.y);
		}

		public override void OnExcuteWithEvent(GopherController fsm) {
			if(fsm.HitGroundAndDie(fsm.diggingBoxRayCaster.CheckCollisionHit(fsm.deadLayers))) {
				return;
			}
			if(InputHandler.Input.GetDigButton()) {
				fsm.movementFsm.Velocity = new Vector2(fsm.movementFsm.Velocity.x, -fsm.movementFsm.yMaxSpeedInGroundDown);
				fsm.movementFsm.ChangeState<MovementFsm.InGroundState>();
				fsm.ChangeState<DiggingState>();
				return;
			}
			if(InputHandler.Input.GetJumpButton()){
				if(upJumpButton) {
					fsm.ChangeState<FirstJumpState>(null);
					return;
				}
			}
			else {
				upJumpButton = true;
			}
		}
		public override void OnExitWithEvent(GopherController fsm) {

		}
		public override string GetStateName(){
			return "Normal";
		}	
    }
    public class FirstJumpState : StateNormal<FirstJumpState>
    {

		float originY = 0;
		bool upJumpButton = false;
		bool firstJumpChance = true;
		GopherController _fsm;
		private float Height {
			get {
				return _fsm.movementFsm.Position.y - originY;
			}
		}
		public override void OnEnterWithEvent(GopherController fsm) {
			originY = fsm.transform.position.y;
			_fsm = fsm;
			fsm.movementFsm.Velocity = new Vector2(fsm.movementFsm.Velocity.x, fsm.firstJumpSpeed);
		}
		public override void OnExcuteWithEvent(GopherController fsm) {
			if(fsm.HitGroundAndDie(fsm.diggingBoxRayCaster.CheckCollisionHit(fsm.deadLayers))) {
				return;
			}
			if(InputHandler.Input.GetJumpButton()) {
				if(!upJumpButton && firstJumpChance) {
					if(_fsm.movementFsm.GetJumpHeight(fsm.firstJumpSpeed) + Height > fsm.firstJumpMaxHeight) {
						firstJumpChance = false;
					} else {
						_fsm.movementFsm.Velocity = new Vector2(_fsm.movementFsm.Velocity.x, fsm.firstJumpSpeed);
					}
				}
				else if(upJumpButton == true && fsm.movementFsm.Velocity.y <= 0) {
					fsm.ChangeState<SecondJumpState>(new SecondJumpState(originY));
				}
			}
			else {
				upJumpButton = true;
			}
		}
        public override string GetStateName()
        {
			return "FirstJump";
        }
    }
    public class SecondJumpState : StateNormal<SecondJumpState>
    {
		float originY = 0;
		float firstOriginY = 0;
		bool upJumpButton = false;
		GopherController _fsm;
		private float Height {
			get {
				return _fsm.movementFsm.Position.y - originY;
			}
		}
		private float firstJumpHeight {
			get {
				return _fsm.movementFsm.Position.y - firstOriginY;
			}
		}
		public SecondJumpState(float firstOriginY) {
			this.firstOriginY = firstOriginY;
		}
		public SecondJumpState() {}
		public override void OnEnterWithEvent(GopherController fsm) {
			originY = fsm.movementFsm.Position.y;
			_fsm = fsm;
			fsm.movementFsm.Velocity = new Vector2(fsm.movementFsm.Velocity.x, fsm.secondJumpSpeed);
		}
		public override void OnExcuteWithEvent(GopherController fsm) {
			if(fsm.HitGroundAndDie(fsm.diggingBoxRayCaster.CheckCollisionHit(fsm.deadLayers))) {
				return;
			}
			if(InputHandler.Input.GetJumpButton()) {
				if(!upJumpButton) {
					if(_fsm.movementFsm.GetJumpHeight(fsm.secondJumpSpeed)> Mathf.Min(fsm.secondJumpMaxHeight - Height, fsm.maxSumHeight - firstJumpHeight)) {
						upJumpButton = true;
					} else {
						_fsm.movementFsm.Velocity = new Vector2(_fsm.movementFsm.Velocity.x, fsm.secondJumpSpeed);
					}
				}
			}
			else {
				upJumpButton = true;
			}
		}
        public override string GetStateName()
        {
			return "SecondJump";
        }
    }
    public class DiggingState : StateSingleton<DiggingState> {
		public override void OnEnterWithEvent(GopherController fsm) {
			fsm.movementFsm.AddEnterEventBeforeExit<MovementFsm.InGroundState>(()=>fsm.ChangeState<FirstJumpState>(null));
		}
		public override void OnExcuteWithEvent(GopherController fsm) {
			if(fsm.HitGroundAndDie(fsm.diggingBoxRayCaster.CheckCollisionHit(fsm.deadLayers))) {
				return;
			}
			if(InputHandler.Input.GetDigButton()) {
				fsm.movementFsm.Velocity = fsm.movementFsm.Velocity + Vector2.down * fsm.yDiggingForce * fsm.movementFsm.UpdateTime;
			}
			var bottomResult = new List<BoxRayCaster.RayTrigger>(fsm.onGroundBoxRayCaster.CheckCollision(fsm.bottomLayers));
			if(bottomResult.Count != 0 && fsm.movementFsm.Velocity.y < 0) {
				fsm.movementFsm.Velocity = new Vector2(fsm.movementFsm.Velocity.x, 0);
			}
			Vector2 direction = fsm.movementFsm.Velocity.normalized;
			float angle = Vector2.Angle(Vector2.right, direction);
			if(direction.y < 0) {
				angle = 360 - angle;
			}
			fsm.diggingBoxRayCaster.gameObject.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
		}
		public override void OnExitWithEvent(GopherController fsm) {
			fsm.movementFsm.RemoveEnterEventBeforeExit<MovementFsm.InGroundState>(()=>fsm.ChangeState<FirstJumpState>(null));
		}
		public override string GetStateName() {
			return "Digging";
		}
	}
	public class DeadState : StateSingleton<DeadState> {
		public override void OnEnterWithEvent(GopherController fsm) {
			EventBus.Notify("PlayerDie");
		}
		public override string GetStateName() {
			return "Dead";
		}
	}
    #endregion
}
