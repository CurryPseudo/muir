using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreeRayCast {
    Vector2 _localCenter;
    Vector2 _worldCenter{
        get{
            return GetWorldPos(_localCenter);
        }
    }
    Vector2 _direction;
    float _step;
    float _rayLength;
    LayerMask _layers;
    Transform _transform;
    public Vector2 GetWorldPos(Vector2 localPos) {
        if(_transform != null){
            return _transform.TransformPoint(localPos);
        }else{
            return localPos;
        }
    }
    public ThreeRayCast(Vector2 center, Vector2 direction, float step, float rayLength, LayerMask layers, Transform transform = null) {
        _localCenter = center;
        _direction = direction;
        _step = step;
        _rayLength = rayLength;
        _layers = layers;
        _transform = transform;
    }
    public IEnumerable<RaycastHit2D> DoNormalRayCast() {
        return DoRayCast(_worldCenter, _direction, _rayLength);
    }
    public IEnumerable<RaycastHit2D> DoRayCast(Vector2 worldCenter, Vector2 rayDirection, float rayLength) {
        rayDirection = rayDirection.normalized;
        Vector2 center = worldCenter - rayDirection * _rayLength * 0.5f;
        Vector2 leftRayCenter = GetLeftCenter(worldCenter);
        Vector2 rightRayCenter = GetRightCenter(worldCenter);
        RaycastHit2D[] leftResult = Physics2D.RaycastAll(leftRayCenter, rayDirection, rayLength, _layers);
        RaycastHit2D[] rightResult = Physics2D.RaycastAll(rightRayCenter, rayDirection, rayLength, _layers);
        RaycastHit2D[] centerResult = Physics2D.RaycastAll(center, rayDirection, rayLength, _layers);
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
    public IEnumerable<RaycastHit2D> DoRayCastEdge(Vector2 lastLocalCenter) {
        Vector2 lastWorldCenter = GetWorldPos(lastLocalCenter);
        Vector2 rayDirection = _worldCenter - lastWorldCenter;
        return DoRayCast(lastWorldCenter, rayDirection, rayDirection.magnitude);
    }
    public RaycastHit2D? GetNearest(){
        float minDistance = float.MaxValue;
        RaycastHit2D? nearestResult = null;
        foreach(var result in DoNormalRayCast()) {
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
        return GetLeftCenter(_worldCenter);
    }

    public Vector2 GetLeftCenter(Vector2 worldCenter) {
        Vector2 left = new Vector2(-_direction.y, _direction.x);
        return worldCenter + left * _step;
    }
    public Vector2 GetRightCenter() {
        return GetRightCenter(_worldCenter);
    }
    public Vector2 GetRightCenter(Vector2 worldCenter){
        Vector2 left = new Vector2(-_direction.y, _direction.x);
        return worldCenter + -left * _step;
    }
}