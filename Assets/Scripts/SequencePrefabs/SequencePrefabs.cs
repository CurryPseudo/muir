using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePrefabs<T> : MonoBehaviour where T : UnityEngine.Object{

	public string prefabFile;
	public List<T> prefabs;
	public int startIndex;
	public void Read() {
		prefabs = new List<T>();
		int index = startIndex;
		T gb = Resources.Load<T>(prefabFile + index.ToString());
		while(gb != null) {
			prefabs.Add(gb);
			index++;
			gb = Resources.Load<T>(prefabFile + index.ToString());
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
