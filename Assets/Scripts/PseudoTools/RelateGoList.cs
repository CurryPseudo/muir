using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
namespace PseudoTools {
    [ExecuteInEditMode]
    public class RelateGoList : MonoBehaviour {
        [ReadOnly]
        public List<GameObject> relateList;
        public bool removeWhenDestroyed = true;
        public void OnDestroy() {
            if(removeWhenDestroyed) {
                relateList.Remove(gameObject);
            }
        }
        public static void Attach(GameObject go, List<GameObject> list) {
            RelateGoList rgl = go.AddComponent<RelateGoList>();
            rgl.relateList = list;
        }
    }
}