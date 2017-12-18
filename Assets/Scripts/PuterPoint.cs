using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuterPoint : MonoBehaviour {

	public Vector3 startPoint;
	public Vector3 endPoint;
	public Vector3 StartPoint{
		get{
			return transform.TransformPoint(startPoint);
		}
		set{
			startPoint = transform.InverseTransformPoint(value);
		}
	}
	public Vector3 EndPoint{
		get{
			return transform.TransformPoint(endPoint);
		}
		set{
			endPoint = transform.InverseTransformPoint(value);
		}
	}
	public event System.Action changePoint;
	public void AjustPoint() {
		changePoint?.Invoke();
	}
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
