using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : SkillType
{
    public TargetSkill(IFindable secondaryTargeting, SkillData data)
    : base(secondaryTargeting, data) { }

    public override bool UseSkill(GameObject primaryTarget, bool isBuffApplied = false)
    {
        if (!primaryTarget)
            return false;

        secondaryTargeting.ChangeCenter(primaryTarget);
        var targets = secondaryTargeting.FindTargets();

        var targetCount = 0;
        foreach (var target in targets)
        {
            if (target != null && ApplySkillActions(target, isBuffApplied))
            {
                var effect = EffectFactory.CreateEffect(AssetId);
                if (effect != null)
                {
                    var autoDestory = effect.gameObject.AddComponent<AutoDestroy>();
                    autoDestory.lifeTime = 1f;
                    var targetFollower = effect.gameObject.AddComponent<TargetFollower>();
                    targetFollower.Target = target;
                }
                targetCount++;
            }
        }

        return targetCount > 0;
    }
}