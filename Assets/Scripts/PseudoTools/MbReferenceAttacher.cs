using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection;
namespace PseudoTools {
	public class MbReferenceAttacher : MonoBehaviour {

		public MonoBehaviour mb;
		public List<ReferenceAttachInfo> attachInfos = new List<ReferenceAttachInfo>();

		[System.Serializable]
		public class ReferenceAttachInfo {
			public string rfMbName;
			public string rfGoName;
			public ReferenceAttachInfo (string rfMbName, string rfGoName) {
				this.rfMbName = rfMbName;
				this.rfGoName = rfGoName;
			}
		}
		public void Awake() {
			foreach(var info in attachInfos) {
				var target = mb.GetType().GetField(info.rfMbName);
				var go = GameObject.Find(info.rfGoName);
				var gcMethod = go.GetType().GetMethod("GetComponent", System.Type.EmptyTypes);
				var reference = gcMethod.MakeGenericMethod(target.FieldType).Invoke(go, null);
				target.SetValue(mb, reference);
			}
		}
	}
}
