using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // private ParticleSystem particle;
    private float destroyTimer = 0f;
    public float lifeTime = 1f;

    private void Awake()
    {
        // particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer >= lifeTime)
            Destroy(gameObject);
    }
}
