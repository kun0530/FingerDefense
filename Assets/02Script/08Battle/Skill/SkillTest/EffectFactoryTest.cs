using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactoryTest : MonoBehaviour
{
    public ParticleSystem effect101;
    private static Dictionary<int, ParticleSystem> effects = new();

    private void Start()
    {
        effects.Add(101, effect101);
    }

    public static ParticleSystem CreateEffect(int effectId, GameObject gameObject)
    {
        if (effects.TryGetValue(effectId, out var value))
        {
            var effect = GameObject.Instantiate(value);
            effect.transform.position = gameObject.transform.position;
            var autoDestroy = effect.gameObject.AddComponent<AutoDestroy>();
            autoDestroy.lifeTime = 2f;
            var TargetFollower = effect.gameObject.AddComponent<TargetFollower>();
            TargetFollower.Target = gameObject;
            return effect;
        }

        return null;
    }
}