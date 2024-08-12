using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Passive Monster Debuff", fileName = "Item.asset")]
public class ItemPassiveDebuffMonster : BaseItem // To-Do: 패시브 버프로 변경
{
    [SerializeField] public ItemDebuffMonster debuff;

    public override void UseItem()
    {
        var stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.STAGE_MANAGER_TAG)?.GetComponent<StageManager>();
        var monsterSpawner = stageManager?.monsterSpawner;
        if (!monsterSpawner)
            return;

        monsterSpawner.onResetMonster += debuff.GiveBuff;
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