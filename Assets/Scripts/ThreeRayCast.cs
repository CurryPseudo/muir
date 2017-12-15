using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreeRayCast {
    Vector2 _localCenter;
    Vector2 _worldCenter{
        get{
            if(_transform != null){
                return _transform.TransformPoint(_localCenter);
            }else{
                return _localCenter;
            }
        }
    }
    Vector2 _direction;
    float _step;
    float _rayLength;
    LayerMask _layers;
    Transform _transform;
    public ThreeRayCast(Vector2 center, Vector2 direction, float step, float rayLength, LayerMask layers, Transform transform = null) {
        _localCenter = center;
        _direction = direction;
        _step = step;
        _rayLength = rayLength;
        _layers = layers;
        _transform = transform;
    }
    public IEnumerable<RaycastHit2D> DoRayCast() {
        _direction = _direction.normalized;
        Vector2 center = _worldCenter - _direction * _rayLength * 0.5f;
        Vector2 leftRayCenter = GetLeftCenter();
        Vector2 rightRayCenter = GetRightCenter();

        RaycastHit2D[] leftResult = Physics2D.RaycastAll(leftRayCenter, _direction, _rayLength, _layers);
        RaycastHit2D[] rightResult = Physics2D.RaycastAll(rightRayCenter, _direction, _rayLength, _layers);
        RaycastHit2D[] centerResult = Physics2D.RaycastAll(center, _direction, _rayLength, _layers);
        foreach(var result in leftResult){
            yield return result;
        }
        foreach(var result in rightResult){
            yield return result;
        }
        foreach(var result in centerResult){
            yield return result;
        }
        yield break;
    }
    public RaycastHit2D? GetNearest(){
        float minDistance = float.MaxValue;
        RaycastHit2D? nearestResult = null;
        foreach(var result in DoRayCast()) {
            if(result.distance < minDistance){
                minDistance = result.distance;
                nearestResult = result;
            }
        }
        return nearestResult;
    }
    public Vector2 GetCenter(){
        return _worldCenter;
    }
    public Vector2 GetLeftCenter(){
        Vector2 left = new Vector2(-_direction.y, _direction.x);
        return _worldCenter + left * _step;
    }
    public Vector2 GetRightCenter(){
        Vector2 left = new Vector2(-_direction.y, _direction.x);
        return _worldCenter + -left * _step;
    }
}