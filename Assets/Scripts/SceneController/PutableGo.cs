using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
namespace SceneController {
	public class PutableGo: MonoBehaviour {
		public Vector2 negativePoint = new Vector2(1,0);
		public Vector2 NegativePointWorld{
			get{
				return transform.TransformPoint(negativePoint);
			}
			set{
				negativePoint = transform.InverseTransformPoint(value);
			}
		}
		public Vector2 positivePoint = new Vector2(-1,0);
		public Vector2 PositivePointWorld{
			get{
				return transform.TransformPoint(positivePoint);
			}
			set{
				positivePoint = transform.InverseTransformPoint(value);
			}
		}
		public void PutNegativeBy(PutableGo pg) {
			PutBy(pg.NegativePointWorld, positivePoint);
		}	
		
		public void PutPositiveBy(PutableGo pg) {
			PutBy(pg.PositivePointWorld, negativePoint);
		}	
		private void PutBy(Vector2 p1, Vector2 p2) {
			Vector3 p1v3 = new Vector3(p1.x, p1.y, 0);
			Vector3 position = p1v3 - transform.TransformVector(p2);
			transform.position = position;
		}
	
	}
}