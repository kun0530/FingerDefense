using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : SkillType
{
    public TargetSkill(IFindable secondaryTargeting, string assetId)
    : base(secondaryTargeting, assetId) { }

    public override bool UseSkill(GameObject primaryTarget)
    {
        if (!primaryTarget)
            return false;

        secondaryTargeting.ChangeCenter(primaryTarget);
        var targets = secondaryTargeting.FindTargets();

        var targetCount = 0;
        foreach (var target in targets)
        {
            if (target != null && ApplySkillActions(target))
            {
                EffectFactoryTest.CreateEffect(assetId, target);
                targetCount++;
            }
        }

        return targetCount > 0;
    }
}