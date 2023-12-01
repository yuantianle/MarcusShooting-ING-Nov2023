using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    private float _smoothSpeed = 0.125f;
    private Vector3 _offset = new Vector3(0, 1.5f, -10f);
    private float _minValueX = -5.04f, _maxValueX = 6.26f;
    private float _minValueY = -0.71f, _maxValueY = 6.11f;
    private float _directionOffset = 0;

    // shake camera
    private float _shakeDuration = 0f;
    private float _shakeMagnitude = 0.7f;
    private float _dampingSpeed = 1.0f;

    private void FixedUpdate()
    {
        // show more front
        Vector3 desiredPosition = target.transform.position + _offset;
        float direction = Input.GetAxis("Horizontal");
        if (direction > 0)
        {
            _directionOffset = 5f;
        }
        else if (direction < 0)
        {
            _directionOffset = -5f;
        }
        desiredPosition.x += _directionOffset;

        // limit camera boundary position
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, _minValueX, _maxValueX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, _minValueY, _maxValueY);

        // smooth camera movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;

        if (_shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * _shakeMagnitude;
            shakeOffset.z = 0; // 仅在 X 和 Y 轴抖动

            desiredPosition += shakeOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

            _shakeDuration -= Time.fixedDeltaTime * _dampingSpeed;
        }
    }
    public void TriggerShake(float duration, float magnitude)
    {
        // first parameter is duration, second is magnitude
        _shakeDuration = duration;
        _shakeMagnitude = magnitude;
    }

}
