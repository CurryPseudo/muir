using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
}