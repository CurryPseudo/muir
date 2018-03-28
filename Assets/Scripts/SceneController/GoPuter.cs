using UnityEngine;
using System.Collections.Generic;
using System;
using SceneController;
namespace SceneController {
    public class GoPuter : MonoBehaviour {
        public Vector2 positiveDirection;
        public Transform baseTransform;
        public float positiveDistance;
        public float negativeDistance;
        public NextPutableGoGetable nextGoGetter;
    }
}