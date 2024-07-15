using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingTargetInCircle : IFindable
{
    public Transform center;
    public float radius;
    public LayerMask targetLayer;

    public FindingTargetInCircle(Transform center, float radius, LayerMask targetLayer)
    {
        this.center = center;
        this.radius = radius;
        
        this.targetLayer = targetLayer;
    }

    public GameObject FindTarget()
    {
        var targets = Physics2D.OverlapCircleAll(center.position, radius, targetLayer);
        GameObject nearObject = null;
        float nearDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target.TryGetComponent<ITargetable>(out var targetable))
            {
                if (!targetable.IsTargetable)
                    continue;
                    
                float distance = Vector2.Distance(target.transform.position, center.position);
                if (distance < nearDistance)
                {
                    nearObject = target.gameObject;
                    nearDistance = distance;
                }
            }
        }

        return nearObject;
    }
}
