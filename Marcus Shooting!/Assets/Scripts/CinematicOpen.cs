using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicOpen : MonoBehaviour
{
    public Material _cinematicMaterial;
    private float _duration = 2.0f; 
    private void Awake()
    {
        //if (!_cinematicMaterial) _cinematicMaterial = GetComponent<SpriteRenderer>().materials[0];
    }

    private void Start()
    {
        StartCoroutine(OpenCinematic());
    }

    IEnumerator OpenCinematic()
    {
        float time = 0;
        float currentRate = _cinematicMaterial.GetFloat("OpenRate");
        while (time < _duration)
        {
            _cinematicMaterial.SetFloat("OpenRate", Mathf.Lerp(currentRate, 1, time / _duration));
            //Debug.Log(_cinematicMaterial.GetFloat("OpenSpeed"));
            time += Time.deltaTime;
            yield return null;
        }

        //Destroy(gameObject);
    }
}
