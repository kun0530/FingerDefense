using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : SkillType
{
    public TargetSkill(IFindable secondaryTargeting)
    : base(secondaryTargeting) { }

    public override void UseSkill(GameObject primaryTarget)
    {
        if (!primaryTarget)
            return;

        secondaryTargeting.ChangeCenter(primaryTarget);
        var targets = secondaryTargeting.FindTargets();

        foreach (var target in targets)
        {
            if (target != null && target.TryGetComponent<IDamageable>(out var damageable))
            {
                ApplySkillActions(damageable);
            }
        }
    }
}