using Sirenix.OdinInspector;
using UnityEngine;
using SceneController;
using System;
using PseudoTools;
namespace SceneController{
    [ExecuteInEditMode]
    public class PutableGoPointEditor : MonoBehaviour {
        [ValidateInput("pgValid", "ShouldAttachByPutableGo!")]
        [ReadOnly]
        public PutableGo putableGo;
        private bool pgValid(PutableGo putableGo) {
            return putableGo != null;
        }
        public void OnEnable() {
            putableGo = GetComponent<PutableGo>();
        }
        [TabGroup("通过子精灵设置Putable Go Point")]
        public DirectionSign xDirection;
        [TabGroup("通过子精灵设置Putable Go Point")]
        public DirectionSign yDirection;
        [ButtonGroup("Putable Go Point")]
        [Button("设置Positive Point")]
        public void SetPositivePoint() { 
            SetPoint((v2 => putableGo.PositivePointWorld = v2));
        }
        [ButtonGroup("Putable Go Point")]
        [Button("设置Negative Point")]
        public void SetNegativePoint() {
            SetPoint((v2 => putableGo.NegativePointWorld = v2));
        }
        public void SetPoint(Action<Vector2> f) {
            f(SubSpriteUltility.GetSubSpritesPoint(xDirection, yDirection, gameObject));
        }  

    }   
}