using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    private float _duration = 0.2f;
    private void Start()
    {
        StartCoroutine(LifecycleCoroutine());
    }

    IEnumerator LifecycleCoroutine()
    {
        yield return new WaitForSeconds(_duration);

        Destroy(gameObject);
    }
}
