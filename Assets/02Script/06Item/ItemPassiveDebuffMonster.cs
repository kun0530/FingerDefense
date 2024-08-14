using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Passive Monster Debuff", fileName = "Item.asset")]
public class ItemPassiveDebuffMonster : BaseItem
{
    [SerializeField] public ItemDebuffMonster debuff;

    public override void UseItem()
    {
        var monsterSpawner = StageMgr?.monsterSpawner;
        if (!monsterSpawner)
            return;

        monsterSpawner.onResetMonster += debuff.GiveBuff;
        if (effectPrefab)
            Instantiate(effectPrefab, new Vector3(0f, -4f, -4f), Quaternion.identity);
    }

    public override void CancelItem()
    {
        var stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.STAGE_MANAGER_TAG)?.GetComponent<StageManager>();
        var monsterSpawner = stageManager?.monsterSpawner;
        if (!monsterSpawner)
            return;

        monsterSpawner.onResetMonster -= debuff.GiveBuff;
    }
}