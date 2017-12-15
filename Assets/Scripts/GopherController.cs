using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GopherController : FiniteStateMachineMonobehaviour<GopherController>{

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
			HintText.Now.Text = movementFsm.current.ToString();
		}
		void OnDestroy() {
		}
	#endregion
	#region Private Methods
		private void HitGroundAndDie() {
			SceneManager.LoadScene(0);
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
			if(fsm.onGroundBoxRayCaster.CheckCollision(fsm.deadLayers)) {
				fsm.HitGroundAndDie();
				return;
			}
			if(fsm.movementFsm.current.ToString() == "OnGround") {
				if(Input.GetMouseButton(0)){
					mouseDownTime += Time.fixedDeltaTime;
				}
				if(mouseDownTime > fsm.holdMouseTime) {
					fsm.movementFsm.ChangeState(MovementFsm.InGroundStateWithEvent.Instance);
					fsm.movementFsm.Velocity = new Vector2(fsm.movementFsm.Velocity.x, -fsm.movementFsm.yMaxSpeedInGroundDown);
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
			if(fsm.diggingBoxRayCaster.CheckCollision(fsm.deadLayers)) {
				fsm.HitGroundAndDie();
				return;
			}
			if(Input.GetMouseButton(0)) {
				fsm.movementFsm.Velocity = fsm.movementFsm.Velocity + Vector2.down * fsm.yDiggingForce * fsm.movementFsm.UpdateTime;
			}
			if(fsm.onGroundBoxRayCaster.CheckCollision(fsm.bottomLayers) && fsm.movementFsm.Velocity.y < 0) {
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
    #endregion
}
