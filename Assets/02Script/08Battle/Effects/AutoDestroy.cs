using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // private ParticleSystem particle;
    private float destroyTimer = 0f;
    [SerializeField] private float lifeTime = 1f;

    private GameObject target;
    public GameObject Target
    {
        get => target;
        set
        {
            if (value == null)
                return;

            isSetTarget = true;
            target = value;
        }
    }

    private bool isSetTarget = false;

    private void Awake()
    {
        // particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer >= lifeTime)
            Destroy(gameObject);

        if (!isSetTarget)
            return;
        
        if (target == null || !target.activeSelf)
            Destroy(gameObject);
        else
            transform.position = target.transform.position;
    }
}
