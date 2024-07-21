using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetSkill : BaseSkill
{
    public SingleTargetSkill(IFindable findable) : base(findable)
    {
    }

    public override void UseSkill()
    {
        var target = targetingMethod.FindTarget();
        if (target != null && target.TryGetComponent<IDamageable>(out var damageable))
        {
            ApplySkillActions(damageable);
        }
    }
}
