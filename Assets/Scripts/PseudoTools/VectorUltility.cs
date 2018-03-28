using UnityEngine;
namespace PseudoTools{
    public class VectorUltility{
        public static Vector2 v32(Vector3 v3) {
            return new Vector2(v3.x, v3.y);
        }
        public static Vector3 v23(Vector3 v2) {
            return new Vector3(v2.x, v2.y, 0);
        }
    }
}