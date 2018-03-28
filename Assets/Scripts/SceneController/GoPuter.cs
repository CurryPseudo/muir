using UnityEngine;
using System.Collections.Generic;
using System;
using SceneController;
using Sirenix.OdinInspector;
using PseudoTools;
using Sirenix.Serialization;
namespace SceneController {
    [ExecuteInEditMode]
    public class GoPuter : MonoBehaviour {
        [OnValueChanged("normalizePositiveDir")]
        public Vector2 positiveDirection = new Vector2(1,0);
        private void normalizePositiveDir() {
            positiveDirection = positiveDirection.normalized;
        }
        [ValidateInput("validTransform")]
        public Transform baseTransform;
        public Vector2 PosDisDir{
            get{
                return VectorUltility.v32(baseTransform.position) + positiveDirection * positiveDistance;
            }
        }
        public Vector2 NegDisDir{
            get{
                return VectorUltility.v32(baseTransform.position) - positiveDirection * negativeDistance;
            }
        }
        private bool validTransform(Transform t) {
            return t != null;
        }
        public float positiveDistance = 10;
        public float negativeDistance = 10;
        public NextPutableGoGetableScriptable nextGoGetter;
        public List<PutableGo> pgList = new List<PutableGo>();
        public void Update() {
            updatePuter();
        }
        [Button("更新")]
        public void updatePuter() {
            Func<PutableGo,Vector2> GetNegativePoint = (pg => pg.NegativePointWorld);
            Func<PutableGo,Vector2> GetPositivePoint = (pg => pg.PositivePointWorld);
            Action<Func<PutableGo,Vector2>,Func<PutableGo,Action<Vector2>>,Func<Vector2,Vector2,bool>,Vector2> extandToRange = (GetPoint, PutBy, Comp, disDir)=>{
                Func<int,PutableGo> getPutableGo = (index => pgList[index]);
                Vector2 bestPoint = GetPoint(getPutableGo(0));
                for(int i = 1; i < pgList.Count; i++) {
                    var pg = getPutableGo(i);
                    if(Comp(GetPoint(pg), bestPoint)) {
                        bestPoint = GetPoint(pg);
                    }
                }
                while(!Comp(bestPoint, disDir)) {
                    var nextPG = NormDir(Instantiate(nextGoGetter.GetNextPutableGo()));
                    nextPG.transform.SetParent(transform, true);
                    pgList.Add(nextPG);
                    PutBy(nextPG)(bestPoint);
                    bestPoint = GetPoint(nextPG);
                }
            };
            Action<Func<PutableGo,Vector2>,Vector2,Func<Vector2,Vector2,bool>> deleteToRange = (GetPoint, disDir, Comp)=>{
                ListUltility.DoIf(pgList, (pg => !Comp(GetPoint(pg), disDir)), (pg)=>{
                    DestroyImmediate(pg.gameObject);
                });
                ListUltility.DeleteIf(pgList, (pg => pg == null));
            };
            ListUltility.DeleteIf(pgList, (pg => pg == null));
            deleteToRange(GetPositivePoint, NegDisDir, Positiver);
            deleteToRange(GetNegativePoint, PosDisDir, Negativer);
            if(pgList.Count == 0) {
                CreateDefaultPG();
            }
            extandToRange(GetNegativePoint,(pg => pg.PutNegativeBy), Negativer, NegDisDir);
            extandToRange(GetPositivePoint,(pg => pg.PutPositiveBy), Positiver, PosDisDir);
        }
        private PutableGo NormDir(PutableGo go) {
            if(Positiver(go.NegativePointWorld, go.PositivePointWorld)) {
                var temp = go.PositivePointWorld;
                go.PositivePointWorld = go.NegativePointWorld;
                go.NegativePointWorld = temp;
            }
            return go;
        }
        private bool Positiver(Vector2 p1, Vector2 p2) {
            return DirectionalDistance(p1, p2) > 0;
        }
        private float DirectionalDistance(Vector2 p1, Vector2 p2) {
            return Vector2.Dot((p1 - p2), positiveDirection);
        }
        private bool Negativer(Vector2 p1, Vector2 p2) {
            return DirectionalDistance(p1, p2) < 0;
        }
        public void CreateDefaultPG() {
            GameObject go = new GameObject("DefaultGoPuter");
            var gp = go.AddComponent<PutableGo>();
            
            float littleOffset = 0.1f;
            gp.PositivePointWorld = PosDisDir + positiveDirection * littleOffset;
            gp.NegativePointWorld = NegDisDir - positiveDirection * littleOffset;

            go.transform.SetParent(transform,false);
            pgList.Add(gp);

        }
    }
}