using UnityEngine;
using SceneController;
namespace SceneController{
    [CreateAssetMenu]
    public class SinglePGGetable : NextPutableGoGetableScriptable
    {
        public PutableGo singlePrefab;
        public override PutableGo GetNextPutableGo()
        {
            var pg = Instantiate(singlePrefab).GetComponent<PutableGo>();
            return pg;
        }
    }
}