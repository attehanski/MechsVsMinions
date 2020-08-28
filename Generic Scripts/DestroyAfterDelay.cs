using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delayTime;

    private float lifeTime;

    void Update()
    {
        if (lifeTime > delayTime)
            Destroy(gameObject);
        lifeTime += Time.deltaTime;
    }
}
