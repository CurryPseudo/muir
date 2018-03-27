using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
namespace SceneController {
	public class PutableGo: MonoBehaviour {
		public Vector2 negativePoint;
		public Vector2 NegativePointWorld{
			get{
				return transform.TransformPoint(negativePoint);
			}
			set{
				negativePoint = transform.InverseTransformPoint(value);
			}
		}
		public Vector2 positivePoint;
		public Vector2 PositivePointWorld{
			get{
				return transform.TransformPoint(positivePoint);
			}
			set{
				positivePoint = transform.InverseTransformPoint(value);
			}
		}
		public void PutNegativeBy(PutableGo pg) {
			PutBy(pg.negativePoint, positivePoint);
		}	
		
		public void PutPositiveBy(PutableGo pg) {
			PutBy(pg.positivePoint, negativePoint);
		}	
		private void PutBy(Vector2 p1, Vector2 p2) {
			Vector3 p1v3 = new Vector3(p1.x, p1.y, 0);
			Vector3 position = p1v3 - transform.TransformVector(p2);
			transform.position = position;
		}
		[Button("Test1")]
		public void Test1() {
			var pg = Instantiate(this).GetComponent<PutableGo>();
			pg.PutNegativeBy(this);
		}
		[Button("Test2")]
		public void Test2() {
			var pg = Instantiate(this).GetComponent<PutableGo>();
			pg.PutPositiveBy(this);
		}
	
	}
}