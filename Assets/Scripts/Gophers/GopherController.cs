using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GopherController : FiniteStateMachineMonobehaviour<GopherController>{

	#region private fields
	#endregion
	#region Inspector
	[Header("Force")]
	public float yDiggingForce = 1;
	[Header("Speed")]
	public float xSpeed = 3;
	public float yJumpSpeed = 4;
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
			ChangeState(NormalState.Instance);
		}
		void Start () {
		}
		protected override void FixedUpdateAfterFSMUpdate() {
			
		}
		void OnDestroy() {
		}
	#endregion
	#region Private Methods
		/* 
		private void HitGroundAndDie(List<BoxRayCaster.RayTrigger> hitTrigger) {
			if(hitTrigger.Count == 0) return;
			movementFsm.DieTriggers = hitTrigger;
			movementFsm.ChangeState(MovementFsm.DeadStateWithEvent.Instance);
			Timer.BeginATimer(2, () => {SceneManager.LoadScene(0);}, this);
			ChangeState(DeadState.Instance);
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
			movementFsm.ChangeState(MovementFsm.DeadStateWithEvent.Instance);
			Timer.BeginATimer(2, LoadDeadScene, this);
			ChangeState(DeadState.Instance);
			return true;
		}
		private void LoadDeadScene() {
			if(GameStatus.Now != null) {
				GameStatus.Now.deadInterface = true;
				SceneManager.LoadSceneAsync("Home");
			}else {
				SceneManager.LoadSceneAsync("MainScene");
			}
		}
		private void ChangeStateToNormal(){
			ChangeState(NormalState.Instance);
		}
		

	#endregion
    #region States
    public class NormalState : State<NormalState>
    {
		float mouseDownTime = 0;
		public override void OnEnter(GopherController fsm){
			fsm.movementFsm.Velocity = new Vector2(fsm.xSpeed, fsm.movementFsm.Velocity.y);
		}

		public override void OnExcute(GopherController fsm) {
			if(fsm.HitGroundAndDie(fsm.diggingBoxRayCaster.CheckCollisionHit(fsm.deadLayers))) {
				return;
			}
			if(fsm.movementFsm.current.ToString() == "OnGround") {
				if(Input.GetMouseButton(0)){
					mouseDownTime += Time.fixedDeltaTime;
				}
				if(mouseDownTime > fsm.holdMouseTime) {
					fsm.movementFsm.Velocity = new Vector2(fsm.movementFsm.Velocity.x, -fsm.movementFsm.yMaxSpeedInGroundDown);
					fsm.movementFsm.ChangeState(MovementFsm.InGroundStateWithEvent.Instance);
					fsm.ChangeState(DiggingState.Instance);
					mouseDownTime = 0;
					return;
				}
				if(Input.GetMouseButtonUp(0)){
					mouseDownTime = 0;
					fsm.movementFsm.Velocity = new Vector2(fsm.movementFsm.Velocity.x, fsm.yJumpSpeed);
				}
			}
			
			
			
		}
		public override void OnExit(GopherController fsm) {

		}
		public override string GetStateName(){
			return "Normal";
		}	
    }
	public class DiggingState : State<DiggingState> {
		public override void OnEnter(GopherController fsm) {
			MovementFsm.InGroundStateWithEvent.Instance.exitEventMap.AddEnterEvent(fsm.movementFsm, fsm.ChangeStateToNormal);
		}
		public override void OnExcute(GopherController fsm) {
			if(fsm.HitGroundAndDie(fsm.diggingBoxRayCaster.CheckCollisionHit(fsm.deadLayers))) {
				return;
			}
			if(Input.GetMouseButton(0)) {
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
		public override void OnExit(GopherController fsm) {
			MovementFsm.InGroundStateWithEvent.Instance.exitEventMap.RemoveEnterEvent(fsm.movementFsm, fsm.ChangeStateToNormal);
		}
		public override string GetStateName() {
			return "Digging";
		}
	}
	public class DeadState : State<DeadState> {
		public override string GetStateName() {
			return "Dead";
		}
	}
    #endregion
}
