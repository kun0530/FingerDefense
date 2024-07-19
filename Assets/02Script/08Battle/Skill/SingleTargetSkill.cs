using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetSkill : BaseSkill
{
    IFindable findBehavior;

    public SingleTargetSkill(IFindable findBehavior)
    {
        this.findBehavior = findBehavior;
    }

    public override void UseSkill()
    {
        var target = findBehavior.FindTarget();
        if (target != null && target.TryGetComponent<IDamageable>(out var damageable))
        {
            ApplySkillActions(damageable);
        }
    }
}
