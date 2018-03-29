using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using PseudoTools;
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
			PutBy(pg.NegativePointWorld, PositivePointWorld);
		}	
		public void PutNegativeBy(Vector2 _negativePointWorld) {
			PutBy(_negativePointWorld, PositivePointWorld);
		}
		public void PutPositiveBy(PutableGo pg) {
			PutBy(pg.PositivePointWorld, NegativePointWorld);
		}	
		public void PutPositiveBy(Vector2 _positivePointWorld) {
			PutBy(_positivePointWorld, NegativePointWorld);
		}
		private void PutBy(Vector2 p1, Vector2 p2) {
			Vector2 position = p1 - p2;
			transform.position = transform.position + VectorUltility.V23(position);
		}
	
	}
}