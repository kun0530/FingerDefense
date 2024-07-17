using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetSkill : BaseSkill
{
    IFindable findBehavior;

    public MultipleTargetSkill(IFindable findBehavior)
    {
        this.findBehavior = findBehavior;
    }

    public override void UseSkill()
    {
        var targets = findBehavior.FindTargets();
        foreach (var target in targets)
        {
            if (target != null && target.TryGetComponent<IDamageable>(out var damageable))
            {
                ApplySkillActions(damageable);
                // EffectFactory.CreateParticleSystem(101, target);
            }
        }
    }
}