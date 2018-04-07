using UnityEngine;
using PseudoTools;
namespace PseudoTools {
    public abstract class ObserverMonoBehaviour : MonoBehaviour {
       public void OnEnable() {
           EventBus.Register(this);
           _OnEnable();
       }
       public abstract void _OnEnable();
       public void OnDisable() {
           EventBus.Deregister(this);
           _OnDisable();
       }
       public abstract void _OnDisable();
    }
}