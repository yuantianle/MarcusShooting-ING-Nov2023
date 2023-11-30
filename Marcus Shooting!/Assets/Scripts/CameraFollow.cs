using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float _minValueX = -15f, _maxValueX = 1.5f;
    public float _minValueY = -1f, _maxValueY = 1f;
    private float _directionOffset = 0;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        float direction = Input.GetAxis("Horizontal");
        if (direction > 0)
        {
            _directionOffset = 5f;
        }
        else if (direction < 0)
        {
            _directionOffset = - 5f;
        }
        desiredPosition.x += _directionOffset;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, _minValueX, _maxValueX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, _minValueY, _maxValueY);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }


}
