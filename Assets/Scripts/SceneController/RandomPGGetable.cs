using UnityEngine;
using SceneController;
using System.Collections.Generic;
namespace SceneController{
    [CreateAssetMenu]
    public class RandomPGGetable : NextPutableGoGetableScriptable
    {
        public List<PutableGo> randomList;
        public override PutableGo GetNextPutableGo()
        {
            int randomIndex = (int)Random.Range(0, randomList.Count);
            return randomList[randomIndex];
        }
    }
}