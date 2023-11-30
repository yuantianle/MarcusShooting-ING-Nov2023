using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float _minValueX = -15f, _maxValueX = 1.5f;
    public float _minValueY = -1f, _maxValueY = 1f;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, _minValueX, _maxValueX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, _minValueY, _maxValueY);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }


}
