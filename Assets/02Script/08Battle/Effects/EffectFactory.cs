using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{
    private static AutoDestroy fireExamplePrefab;
    public AutoDestroy firePrefab;

    public static GameObject CreateParticleSystem(int id, GameObject target)
    {
        // id에 따른 분기
        var effect = GameObject.Instantiate(fireExamplePrefab);
        effect.transform.position = target.transform.position;
        effect.Target = target;

        return effect.gameObject;
    }

    private void Awake()
    {
        fireExamplePrefab = firePrefab;
    }
}
