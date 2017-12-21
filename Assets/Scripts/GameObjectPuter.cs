using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPuter : MonoBehaviour {

	public float tooFarDestroyDistance;
	public float tooCloseCreateDistance;
	public List<GameObject> putPrefabs;
	public List<GameObject> nowInstances;
	public GameObject newestInstance;
	public SequenceSpritesWithIndex sequenceSprites;
	public Transform putTransformParent;
	public MovementFsm parallaxMovement;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		DestroyTooFarObject();
		CreateObject();
	}
	public void DestroyTooFarObject() {
		List<GameObject> removeList = new List<GameObject>();
		foreach(var ground in nowInstances) {
			PuterPoint points = ground.GetComponent<PuterPoint>();
			float distance = (points.EndPoint - transform.position).magnitude;
			if(distance > tooFarDestroyDistance) {
				removeList.Add(ground);
				Destroy(ground);
			}
		}
		foreach(var ground in removeList) {
			nowInstances.Remove(ground);
		}
	}
	public void CreateObject() {
		PuterPoint points = newestInstance.GetComponent<PuterPoint>();
		float distance = (points.StartPoint - transform.position).magnitude;
		while(distance < tooCloseCreateDistance) {
			GameObject prefab = putPrefabs[(int)Random.Range(0, putPrefabs.Count)];
			GameObject newGround = Instantiate(prefab);
			if(sequenceSprites != null) {
				newGround.GetComponent<SequenceSpriteSetter>().SequenceSpritesWithIndex = sequenceSprites;
			}
			Vector3 position = points.EndPoint - newGround.transform.TransformVector(newGround.GetComponent<PuterPoint>().startPoint);
			newGround.transform.position = position;
			newGround.transform.SetParent(putTransformParent, true);
			nowInstances.Add(newGround);
			newestInstance = newGround;
			points = newestInstance.GetComponent<PuterPoint>();
			distance = (points.StartPoint - transform.position).magnitude;
			ParallaxMove parallax = newGround.GetComponent<ParallaxMove>();
			if(parallax != null) {
				parallax.movement = parallaxMovement;
				parallax.sequenceSprites = sequenceSprites;
			}
			DisableMonoBehaviourByPlayerDie disableEvent = newGround.GetComponent<DisableMonoBehaviourByPlayerDie>();
			if(disableEvent != null) {
				disableEvent.movementFsm = parallaxMovement;
			}
		}
	}
}
