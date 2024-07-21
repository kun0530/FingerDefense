using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetSkill : BaseSkill
{
    public MultipleTargetSkill(IFindable findable) : base(findable)
    {
    }

    public override void UseSkill()
    {
        var targets = targetingMethod.FindTargets();
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