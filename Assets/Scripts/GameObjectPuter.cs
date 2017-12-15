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
			Vector3 position = points.EndPoint - prefab.transform.TransformVector(prefab.GetComponent<PuterPoint>().startPoint);
			GameObject newGround = Instantiate(prefab, position, Quaternion.identity);
			newGround.transform.SetParent(putTransformParent, true);
			if(sequenceSprites != null) {
				newGround.GetComponent<SequenceSpriteSetter>().SequenceSpritesWithIndex = sequenceSprites;
			}
			nowInstances.Add(newGround);
			newestInstance = newGround;
			points = newestInstance.GetComponent<PuterPoint>();
			distance = (points.StartPoint - transform.position).magnitude;
		}
	}
}
