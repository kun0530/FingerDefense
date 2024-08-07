using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemFrozen : IItem
{
    public bool UseItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters.Length == 0)
            return false;

        foreach (var monster in monsters)
        {
            if (!monster.TryGetComponent<MonsterController>(out var controller))
                continue;

            var moveSpeed = controller.Status.Data.MoveSpeed;

            var buffData = new BuffData();
            buffData.BuffActions.Add(((int)BuffType.MOVE_SPEED, -moveSpeed * 0.1f));
            // buffData.EffectNo = 101;
            controller.TryTakeBuff(buffData, out var buff, true);
        }

        return true;
    }
}