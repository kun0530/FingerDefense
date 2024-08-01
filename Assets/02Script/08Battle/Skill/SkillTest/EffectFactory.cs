using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectFactory
{
    private static string EffectFile = "Effects/{0}";

    public static ParticleSystem CreateEffect(int effectId)
    {
        var assetTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        var effectFilePath = string.Format(EffectFile, assetTable.Get(effectId));
        var effectResource = Resources.Load<ParticleSystem>(effectFilePath);
        if (effectResource == null)
            return null;
        var effect = GameObject.Instantiate(effectResource);

        return effect;
    }

    public static ParticleSystem CreateEffect(string effectId, GameObject gameObject, float lifeTime = 1f)
    {
        var effectResource = Resources.Load<ParticleSystem>(string.Format(EffectFile, effectId));
        if (effectResource == null)
            return null;
        var effect = GameObject.Instantiate(effectResource);
        // var effect = GameObject.Instantiate(value);
        effect.transform.position = gameObject.transform.position;
        var autoDestroy = effect.gameObject.AddComponent<AutoDestroy>();
        autoDestroy.lifeTime = lifeTime;
        var TargetFollower = effect.gameObject.AddComponent<TargetFollower>();
        TargetFollower.Target = gameObject;
        return effect;
    }

    public static ParticleSystem CreateEffect(string effectId)
    {
        var effectResource = Resources.Load<ParticleSystem>(string.Format(EffectFile, effectId));
        if (effectResource == null)
            return null;

        return GameObject.Instantiate(effectResource);
    }
}