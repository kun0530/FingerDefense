using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSkill : BaseSkill
{
    public InstantSkill(SkillData skillData, SkillType skillType, IFindable targetingMethod)
    : base(skillData, skillType, targetingMethod) { }

    public override bool UseSkill()
    {
        var target = targetingMethod.FindTarget();
        if (target == null || skillType == null)
            return false;

        if (skillType.UseSkill(target))
        {
            IsSkillReady = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}