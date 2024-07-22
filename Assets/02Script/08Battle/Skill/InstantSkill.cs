using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSkill : BaseSkill
{
    public InstantSkill(SkillType baseSkill, IFindable targetingMethod)
    : base(baseSkill, targetingMethod)
    {
    }

    public override void UseSkill()
    {
        var targets = targetingMethod.FindTargets();
        foreach (var target in targets)
        {
            if (target != null)
                baseSkill?.UseSkill(target);
        }
    }
}
