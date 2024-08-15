using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Passive Monster Debuff", fileName = "Item.asset")]
public class ItemPassiveDebuffMonster : BaseItem
{
    [Header("이펙트")]
    public EffectController effectPrefab;
    [Header("몬스터 디버프")]
    [SerializeField] public ItemDebuffMonster debuff;

    private EffectController activeEffect;

    public override void UseItem()
    {
        var monsterSpawner = StageMgr?.monsterSpawner;
        if (!monsterSpawner)
            return;

        monsterSpawner.onResetMonster += debuff.GiveBuff;
        if (effectPrefab)
            activeEffect = Instantiate(effectPrefab);
    }

    public override void CancelItem()
    {
        if (activeEffect)
            Destroy(activeEffect.gameObject);

        var stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.STAGE_MANAGER_TAG)?.GetComponent<StageManager>();
        var monsterSpawner = stageManager?.monsterSpawner;
        if (!monsterSpawner)
            return;

        monsterSpawner.onResetMonster -= debuff.GiveBuff;
    }
}