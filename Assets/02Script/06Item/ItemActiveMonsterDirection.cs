using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Monster Direction Change", fileName = "Item.asset")]
public class ItemActiveMonsterDirection : BaseItem
{
    [Tooltip("몬스터의 방향 조정: -1인 경우, 반대 방향")]
    public float directionMultiplier;
    public float lastingTime;
    private float timer;

    public override bool UseItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters == null)
            return false;
        foreach (var monster in monsters)
        {
            if (monster.TryGetComponent<MonsterController>(out var controller))
            {
                if (controller.CurrentState == typeof(AttackState))
                    controller.TryTransitionState<PatrolState>();
                controller.directionMultiplier = directionMultiplier;
            }
        }
        timer = 0f;
        return true;
    }

    public override bool CancelItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters == null)
            return false;
        foreach (var monster in monsters)
        {
            if (monster.TryGetComponent<MonsterController>(out var controller))
                controller.directionMultiplier = 1f;
        }

        timer = 0f;
        return true;
    }

    public override void UpdateItem()
    {
        timer += Time.deltaTime;
        if (timer >= lastingTime)
        {
            CancelItem();
        }
    }
}
