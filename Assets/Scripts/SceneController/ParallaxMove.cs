using UnityEngine;
using Sirenix.OdinInspector;
using PseudoTools;
using System;
namespace SceneController {
    [ExecuteInEditMode]
    public class ParallaxMove : CouldDisableEditorUpdateMonoBehaviour {
        [ValidateInput("validTransform")]
        public Transform baseTransform;
        public Vector2 degrees = new Vector2(1,1);
        private Vector3 lastPosition;
        private bool validTransform(Transform transform) {
            return transform != null;
        }
        public void Start() {
            lastPosition = baseTransform.position;
        }
        public override void _Update() {
            transform.position = transform.position + VectorUtility.Multiply((baseTransform.position - lastPosition), VectorUtility.V23(degrees));
            lastPosition = baseTransform.position;
        }
        [TabGroup("通过某Go的子精灵来设置Degrees")]
        public DirectionSign xDirection;
        [TabGroup("通过某Go的子精灵来设置Degrees")]
        public DirectionSign yDirection;
        [TabGroup("通过某Go的子精灵来设置Degrees")]
        public GameObject targetGo;
        [ButtonGroup("设置Degrees")]
        public void SetDegrees() {
            if(targetGo != null) {
                Func<GameObject, Func<DirectionSign,DirectionSign>, Vector2> getPoint = (go, Fd) => {
                    return SubSpriteUtility.GetSubSpritesPoint(Fd(xDirection), Fd(yDirection), go);
                };

                Func<DirectionSign, DirectionSign> reverseDir = (d) => {
                    return (DirectionSign)(-(int)d);
                };
                Func<GameObject, Vector2> getVector = (go => getPoint(go, (d=>d)) - getPoint(go, reverseDir));
                
                Func<Func<float>,Func<float>,float> solveComponent = (GetV1, GetV2) => {
                    if(GetV2() == 0) {
                        return 0;
                    }
                    return 1 - GetV1() / GetV2();
                };
                Func<Vector2, Vector2, Vector2> getDegrees = (v1, v2) => {
                    float x = solveComponent(() => v1.x, () => v2.x);
                    float y = solveComponent(() => v1.y, () => v2.y);
                    return new Vector2(x,y);
                };
                degrees = getDegrees(getVector(gameObject), getVector(targetGo));
            }
        }
    }
}