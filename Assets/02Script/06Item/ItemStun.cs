using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStun : IItem
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
            buffData.LastingTime = 2f;
            buffData.BuffActions.Add(((int)BuffType.MOVE_SPEED, -moveSpeed));
            // buffData.EffectNo = 101;
            controller.TakeBuff(buffData);
        }

        return true;
    }
}
