using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingTargetInCircle : IFindable
{
    public Transform center;
    public float radius;
    public LayerMask targetLayer;

    public FindingTargetInCircle(Transform center, float radius)
    {
        this.center = center;
        this.radius = radius;

        targetLayer = 1 << LayerMask.NameToLayer("Player");
    }

    public IControllable FindTarget()
    {
        var targets = Physics2D.OverlapCircleAll(center.position, radius, targetLayer);
        PlayerCharacterController nearCollider = null;
        float nearDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target.TryGetComponent<PlayerCharacterController>(out var playerCharacter))
            {
                if (playerCharacter.MonsterCount == 2)
                    continue;
                    
                float distance = Vector2.Distance(playerCharacter.transform.position, center.position);
                if (distance < nearDistance)
                {
                    nearCollider = playerCharacter;
                    nearDistance = distance;
                }
            }
        }

        return nearCollider;
    }
}
