using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private float lifeTime;
    public float LifeTime
    {
        get => lifeTime;
        set
        {
            if (value < 0f)
                return;

            lifeTime = value;
            timer = 0f;
            isAutoDestroy = true;
        }
    }
    private bool isAutoDestroy = false;
    private float timer = 0f;

    private void Update()
    {
        AutoDestroy();
    }

    private void AutoDestroy()
    {
        if (!isAutoDestroy)
            return;

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
