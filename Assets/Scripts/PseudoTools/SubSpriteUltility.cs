using UnityEngine;
using PseudoTools;
using System;
namespace PseudoTools {
    public class SubSpriteUltility {
        public static Vector2 GetSubSpritesPoint(DirectionSign xDirection, DirectionSign yDirection, GameObject go) {
            Vector2 resultPoint;

            SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
            Func<Func<Bounds,Vector3>,Func<Vector3[],Vector3>,Vector3> getEdge = (edgeFunction,maxFunction) => {
                Vector3[] edges = new Vector3[renderers.Length];
                int index = 0;
                foreach(var renderer in renderers) {
                    Vector3 edge = edgeFunction(renderer.bounds);
                    edges[index] = edge; 
                    index++;
                }
                return maxFunction(edges);
            };
            Func<Bounds,Vector3> edgeFunc = (bounds)=>{
                return bounds.center + new Vector3(bounds.size.x / 2 * (int)xDirection, bounds.size.y / 2 * (int)yDirection, 0);
            };
            Func<Vector3[], Vector3> maxFunc = (positions)=>{
                Func<DirectionSign,Func<Vector3,float>, float> metaMax = null;
                metaMax = (d,v3Component) => {
                    if(d == DirectionSign.Stay) {
                        return (metaMax(DirectionSign.Forward, v3Component) + metaMax(DirectionSign.Backward, v3Component)) / 2;
                    }
                    float result = d == DirectionSign.Forward?float.MinValue:float.MaxValue;
                    float sign = (int)d;
                    foreach(var position in positions) {
                        if((v3Component(position) - result) * sign > 0) {
                            result = v3Component(position);
                        }
                    }
                    return result;
                };
                Func<Vector3,float> v3x = (v3 => v3.x);
                Func<Vector3,float> v3y = (v3 => v3.y);
                float rx = metaMax(xDirection,v3x);
                float ry = metaMax(yDirection,v3y);
                return new Vector3(rx,ry,0);
            };
            Vector3 resultv3 = getEdge(edgeFunc,maxFunc);
            resultPoint = new Vector2(resultv3.x, resultv3.y);
            return resultPoint;
        }
    }
}