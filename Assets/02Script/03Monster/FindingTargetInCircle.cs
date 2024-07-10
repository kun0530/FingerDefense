using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingTargetInCircle<T> : IFindable where T : MonoBehaviour, IControllable
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

    public IControllable FindTarget()
    {
        var targets = Physics2D.OverlapCircleAll(center.position, radius, targetLayer);
        T nearCollider = null;
        float nearDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target.TryGetComponent<T>(out var controller))
            {
                if (!controller.IsTargetable)
                    continue;
                    
                float distance = Vector2.Distance(controller.transform.position, center.position);
                if (distance < nearDistance)
                {
                    nearCollider = controller;
                    nearDistance = distance;
                }
            }
        }

        return nearCollider;
    }
}
