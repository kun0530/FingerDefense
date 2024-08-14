using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Monster Direction Change", fileName = "Item.asset")]
public class ItemActiveMonsterDirection : ActiveItem
{
    public float speedMultiplier;

    public override void UseItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters == null)
            return;

        foreach (var monster in monsters)
        {
            if (monster.TryGetComponent<MonsterController>(out var controller))
            {
                controller.TryTransitionState<BackMoveState>();
                controller.speedMultiplier = speedMultiplier;

                if (effectPrefab)
                {
                    var effect = Instantiate(effectPrefab);
                    controller.AddEffect(effect);
                    effect.LifeTime = duration;
                }
            }
        }

        base.UseItem();
    }

    public override void CancelItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters == null)
            return;
        foreach (var monster in monsters)
        {
            if (monster.TryGetComponent<MonsterController>(out var controller))
                controller.TryTransitionState<PatrolState>();
            controller.speedMultiplier = 1f;
        }

        base.CancelItem();
    }
}
