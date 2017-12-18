
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenShake : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
    private Vector3 originPosition;
    private float shakeValue = 0;
    private bool lastShakeStatus;
    private Vector3 shakeCameraPos(Vector3 originPos) {
        Vector3 deltaPosition = Vector3.zero;
        if(shake) {
            Vector3 range = shakeRange;
            range = Vector3.Scale(range, getRandomValue(-1, 1, shakeValue));
            deltaPosition += range;
        }
        return deltaPosition + originPos;
    }
    private float getRandomValue(float min, float max, float x, float y) {
        return Mathf.Lerp(min, max, Mathf.PerlinNoise(x, y));
    }
    private Vector2 getRandomValue(float min, float max, float value) {
        Vector2 result = new Vector2(getRandomValue(min, max, 0, value), getRandomValue(min, max, value, 0));
        return result;
    }
	#endregion	
	#region Inspector
    public CameraFollow follow;
    public Vector2 shakeRange;
    public float shakeSpeed;
    public bool shake;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        follow = GetComponent<CameraFollow>();
        follow.effect += shakeCameraPos;
    }

    void Update() {
        if(shake && !lastShakeStatus) {
            shakeValue = Random.Range(0,10);
        }
        lastShakeStatus = shake;
        shakeValue += Time.deltaTime * shakeSpeed;
    }
	#endregion
	#region Public Method
	#endregion
}
