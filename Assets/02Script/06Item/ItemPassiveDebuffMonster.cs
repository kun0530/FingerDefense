using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Passive Monster Debuff", fileName = "Item.asset")]
public class ItemPassiveDebuffMonster : ItemDebuffMonster // To-Do: 패시브 버프로 변경
{
    public override bool IsPassive { get => true; }

    public override bool UseItem()
    {
        var stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.STAGE_MANAGER_TAG)?.GetComponent<StageManager>();
        var monsterSpawner = stageManager?.monsterSpawner;
        if (!monsterSpawner)
            return false;

        monsterSpawner.onResetMonster += GiveBuff;
        return true;
    }

    public override bool CancelItem()
    {
        var stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.STAGE_MANAGER_TAG)?.GetComponent<StageManager>();
        var monsterSpawner = stageManager?.monsterSpawner;
        if (!monsterSpawner)
            return false;

        monsterSpawner.onResetMonster -= GiveBuff;
        return true;
    }
}