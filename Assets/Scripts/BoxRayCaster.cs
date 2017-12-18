using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoxRayCaster : MonoBehaviour {
	#region Internal Classes
		public class RayTrigger{
			public enum Direction{
				UP, DOWN, LEFT, RIGHT
			}
			Vector2 rayCenter;
			Vector2 rayDirection;
			float rayStep;
			BoxRayCaster _parent;
			Direction _direction;
			HashSet<Collider2D> collidersIn = new HashSet<Collider2D>();
			ThreeRayCast threeRayCast;
			Vector2 lastRayCenter;
			public System.Action<RaycastHit2D> enterAction;
			public System.Action<Collider2D> exitAction;
			public string CollisionStatus {
				get{
					string result = "Status:\n";
					foreach(var collider in collidersIn) {
						string colliderString = "Name: " + collider.name
							 + ", Layer: " + LayerMask.LayerToName(collider.gameObject.layer) + "\n";
						result += colliderString;
					}
					return result;
				}
			}
			public Vector2 RayDirection{
				get{
					return rayDirection.normalized;
				}
			}
			public float RayStep{
				get{
					return rayStep;
				}
			}
			public float GetNeedVector2Value(Vector2 v2) {
				if(RayDirection.x == 0) {
					return v2.y;
				}else{
					return v2.x;
				}
			}
			public float GetWorldDirectionValue(Vector2 centerWorldPosition, Vector2 hitPoint) {
				BoxCollider2D box = _parent.boxCollider2D; 
				Vector2 boxSizeHalf = box.size / 2;
				Vector2 localBoxDirection = new Vector2(boxSizeHalf.x * RayDirection.x, boxSizeHalf.y * RayDirection.y);
				Vector2 worldBoxDirection = _parent.transform.TransformDirection(localBoxDirection);
				Vector2 edgeWorldPosition = centerWorldPosition + worldBoxDirection;
			 	float worldDirectionValue = GetNeedVector2Value(hitPoint) - GetNeedVector2Value(edgeWorldPosition);
				return worldDirectionValue;
			}

			public RayTrigger(BoxRayCaster parent, Direction direction) {
				_parent = parent;
				_direction = direction;
				SetRayValue();
			}
			public ICollection<Collider2D> GetCollidersInside(){
				List<Collider2D> colliders = new List<Collider2D>(collidersIn);
				return colliders;
			}
			public void Update() {
				
				SetRayValue();
				HashSet<Collider2D> newCollidersIn = new HashSet<Collider2D>();
				foreach(var hit in threeRayCast.DoNormalRayCast()) {
					newCollidersIn.Add(hit.collider);
					if(!collidersIn.Contains(hit.collider)) {
						EnterAction(hit);
					}
				}
				foreach(var collider in collidersIn) {
					if(!newCollidersIn.Contains(collider)) {
						ExitAction(collider);
					}
				}
				collidersIn = newCollidersIn;
			}
			void ExitAction(Collider2D collider) {
				if(exitAction != null) {
					exitAction(collider);
				}
			}
			void EnterAction(RaycastHit2D hit) {
				if(enterAction != null) {
					enterAction(hit);
				}
			}
			void SetRayValue(){
				if(_direction == Direction.DOWN) {
					rayCenter = _parent.boxCollider2D.offset - _parent.boxCollider2D.size / 2;
					rayCenter.x = _parent.boxCollider2D.offset.x; 
					rayCenter += Vector2.up * _parent.rayLength;
					rayDirection = Vector2.down;
					rayStep = _parent.boxCollider2D.size.x / 2;
				}
				else if(_direction == Direction.LEFT) {
					rayCenter = _parent.boxCollider2D.offset;
					rayCenter.x = rayCenter.x - _parent.boxCollider2D.size.x / 2;
					rayCenter += Vector2.right * _parent.rayLength;
					rayDirection = Vector2.left;
					rayStep = _parent.boxCollider2D.size.y / 2;
				}
				else if(_direction == Direction.RIGHT) {
					rayCenter = _parent.boxCollider2D.offset;
					rayCenter.x = rayCenter.x + _parent.boxCollider2D.size.x / 2; 
					rayCenter += Vector2.left * _parent.rayLength;
					rayDirection = Vector2.right;
					rayStep = _parent.boxCollider2D.size.y / 2;
				}
				else if(_direction == Direction.UP) {
					rayCenter = _parent.boxCollider2D.offset +_parent.boxCollider2D.size / 2;
					rayCenter.x = _parent.boxCollider2D.offset.x; 
					rayCenter += Vector2.down * _parent.rayLength;
					rayDirection = Vector2.up;
					rayStep = _parent.boxCollider2D.size.x / 2;
				} 
				rayCenter += rayDirection * _parent.rayLength / 2;
				rayStep -= _parent.rayStepOffSet;
				Quaternion rotation = _parent.transform.rotation;
				Vector3 rayDirectionV3 = new Vector3(rayDirection.x, rayDirection.y, 0);
				rayDirectionV3 = rotation * rayDirectionV3;
				rayDirection = new Vector2(rayDirectionV3.x, rayDirectionV3.y);
				threeRayCast = new ThreeRayCast(rayCenter, rayDirection, rayStep, _parent.rayLength, _parent.rayLayer, _parent.transform);
			}
			public void DrawRayGizmos() {
				SetRayValue();
				float size = 0.025f;
				Gizmos.DrawCube(threeRayCast.GetCenter(), size * new Vector3(1, 1, 1));
				Gizmos.DrawCube(threeRayCast.GetLeftCenter(), size * new Vector3(1, 1, 1));
				Gizmos.DrawCube(threeRayCast.GetRightCenter(), size * new Vector3(1, 1, 1));
				Vector2 offSetCenter = threeRayCast.GetCenter();
				Vector2 targetCenter = offSetCenter + rayDirection * _parent.rayLength;
				if(collidersIn.Count == 0) {
					Gizmos.color = Color.green;
				}
				else {
					Gizmos.color = Color.blue;
					foreach(var collider in collidersIn) {
					}
				}
				Gizmos.DrawLine(offSetCenter, targetCenter);

			}
			public bool CheckCollision(LayerMask layers) {
				foreach(var collider in GetCollidersInside()) {
					if(LayerMaskUtil.IsIncluded(layers, collider.gameObject.layer)) {
						return true;
					}
				}
				return false;
			}
			public IEnumerable<RaycastHit2D> GetHits(LayerMask layers) {
				SetRayValue();
				return GetHits(layers, threeRayCast.DoNormalRayCast());
			}
			private IEnumerable<RaycastHit2D> GetHits(LayerMask layerMask, IEnumerable<RaycastHit2D> hits) {
				foreach(var hit in hits) {
					if(LayerMaskUtil.IsIncluded(layerMask, hit.collider.gameObject.layer)) {
						yield return hit;
					}
				}
				yield break;
			}
			public IEnumerable<RaycastHit2D> GetEdgeHits(LayerMask layers) {
				lastRayCenter = rayCenter;
				SetRayValue();
				return GetHits(layers, threeRayCast.DoRayCastEdge(lastRayCenter));
			}
		}
	#endregion

	#region Inspector
		public float rayLength = 0.02f;
		public LayerMask rayLayer;
		public BoxCollider2D boxCollider2D;
		public float rayStepOffSet = 0.05f;
	#endregion
	#region Public Properties
	public RayTrigger Up{
		get{
			if(up == null){
				up = new RayTrigger(this, RayTrigger.Direction.UP);
			}
			return up;
		}
	}
	public RayTrigger Down{
		get{
			if(down == null){
				down = new RayTrigger(this, RayTrigger.Direction.DOWN);
			}
			return down;
		}
	}
	public RayTrigger Left{
		get{
			if(left == null){
				left = new RayTrigger(this, RayTrigger.Direction.LEFT);
			}
			return left;
		}
	}
	public RayTrigger Right{
		get{
			if(right == null){
				right = new RayTrigger(this, RayTrigger.Direction.RIGHT);
			}
			return right;
		}
	}
	#endregion
	#region Private Fields
		RayTrigger up;
		RayTrigger down;
		RayTrigger left;
		RayTrigger right;

	#endregion
	#region Private methods
		
	#endregion
	#region Public Methods
	public IEnumerable<RaycastHit2D> CheckCollisionEdgeHit(LayerMask mask) {
		foreach(var hit in Up.GetEdgeHits(mask)) {
			yield return hit;
		}
		foreach(var hit in Down.GetEdgeHits(mask)) {
			yield return hit;
		}
		foreach(var hit in Left.GetEdgeHits(mask)) {
			yield return hit;
		}
		foreach(var hit in Right.GetHits(mask)) {
			yield return hit;
		}
		yield break;

	}
	public IEnumerable<RaycastHit2D> CheckCollisionHit(LayerMask mask) {
		foreach(var hit in Up.GetHits(mask)) {
			yield return hit;
		}
		foreach(var hit in Down.GetHits(mask)) {
			yield return hit;
		}
		foreach(var hit in Left.GetHits(mask)) {
			yield return hit;
		}
		foreach(var hit in Right.GetHits(mask)) {
			yield return hit;
		}
		yield break;

	}
	public IEnumerable<RayTrigger> CheckCollision(LayerMask mask) {

		if(Up.CheckCollision(mask)) {
			yield return Up;
		}
		if(Down.CheckCollision(mask)) {
			yield return Down;
		}
		if(Left.CheckCollision(mask)) {
			yield return Left;
		}
		if(Right.CheckCollision(mask)) {
			yield return Right;
		}
		yield break;
	}
	#endregion
	#region MonoBehaviour Methods
		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			boxCollider2D = GetComponent<BoxCollider2D>();
					}
		/// <summary>
		/// Callback to draw gizmos only if the object is selected.
		/// </summary>
		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Right.DrawRayGizmos();
			Gizmos.color = Color.blue;
			Left.DrawRayGizmos();
			Gizmos.color = Color.green;
			Up.DrawRayGizmos();
			Gizmos.color = Color.red;
			Down.DrawRayGizmos();
			//Vector2 center = transform.TransformPoint(boxCollider2D.offset);
			//Down.GetWorldDirectionValue(center, Vector2.zero);
		}
			/// <summary>
		/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
		/// </summary>
		void FixedUpdate()
		{
			Up.Update();
			Down.Update();
			Left.Update();
			Right.Update();
		}
	#endregion
}
