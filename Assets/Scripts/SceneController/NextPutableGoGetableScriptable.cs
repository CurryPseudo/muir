using UnityEngine;
using SceneController;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
namespace SceneController{
    public abstract class NextPutableGoGetableScriptable : SerializedScriptableObject, NextPutableGoGetable 
    {
        public abstract PutableGo GetNextPutableGo();
    }
}