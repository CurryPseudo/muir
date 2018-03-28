using UnityEngine;
using SceneController;
namespace SceneController{
    [CreateAssetMenu]
    public class SinglePGGetable : NextPutableGoGetableScriptable
    {
        public PutableGo singlePrefab;
        public override PutableGo GetNextPutableGo()
        {
            return singlePrefab;
        }
    }
}