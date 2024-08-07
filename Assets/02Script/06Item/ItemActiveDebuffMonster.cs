using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Monster Debuff", fileName = "Item.asset")]
public class ItemActiveDebuffMonster : ItemDebuffMonster
{
    public override bool UseItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters == null)
            return false;
        foreach (var monster in monsters)
        {
            if (monster.TryGetComponent<MonsterController>(out var controller))
                GiveBuff(controller);
        }
        return true;
    }

    public override bool CancelItem()
    {
        // 모든 몬스터에게 버프 취소
        return true;
    }
}
