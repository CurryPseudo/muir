using UnityEngine;
namespace PseudoTools{
    public class VectorUtility{
        public static Vector2 V32(Vector3 v3) {
            return new Vector2(v3.x, v3.y);
        }
        public static Vector3 V23(Vector3 v2) {
            return new Vector3(v2.x, v2.y, 0);
        }
        public static Vector3 Multiply(Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
        public static Vector2 Multiply(Vector2 v1, Vector2 v2) {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }
        public static Vector3 Division(Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }
        public static Vector2 Division(Vector2 v1, Vector2 v2) {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }
    }
}