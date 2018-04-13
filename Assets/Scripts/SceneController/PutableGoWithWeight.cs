using UnityEngine;
using SceneController;
using System;
namespace SceneController {
    public class PutableGoWithWeight : MonoBehaviour, IComparable<PutableGoWithWeight>{
        public PutableGo putableGo;
        public float weight;

        int IComparable<PutableGoWithWeight>.CompareTo(PutableGoWithWeight other)
        {
            return weight.CompareTo(other.weight);
        }
    }
}