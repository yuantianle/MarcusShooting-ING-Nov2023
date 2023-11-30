using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSound : MonoBehaviour
{
    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!_source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
