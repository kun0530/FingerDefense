using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Monster Direction Change", fileName = "Item.asset")]
public class ItemActiveMonsterDirection : ActiveItem
{
    [Tooltip("몬스터의 방향 조정: -1인 경우, 반대 방향")]
    public float directionMultiplier;

    public override void UseItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters == null)
            return;

        foreach (var monster in monsters)
        {
            if (monster.TryGetComponent<MonsterController>(out var controller))
            {
                if (controller.CurrentState == typeof(AttackState))
                    controller.TryTransitionState<PatrolState>();
                controller.directionMultiplier = directionMultiplier;
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
                controller.directionMultiplier = 1f;
        }

        base.CancelItem();
    }
}
