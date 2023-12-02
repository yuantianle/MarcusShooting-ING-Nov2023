using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        transform.position = new Vector3(-_camera.transform.position.x*0.2f, -_camera.transform.position.y*0.2f, transform.position.z);
    }
}
