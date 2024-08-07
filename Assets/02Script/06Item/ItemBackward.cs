using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBackward : IItem
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

            controller.speedMultiplier *= -1f;
        }

        return true;
    }
}
