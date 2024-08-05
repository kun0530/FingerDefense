using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : BaseSkill
{
    public ProjectileSkill(SkillData skillData, SkillType skillType, IFindable targetingMethod, GameObject caster)
    : base(skillData, skillType, targetingMethod, caster) { }

    public override bool UseSkill(bool isBuffApplied = false)
    {
        var target = targetingMethod.FindTarget();
        if (target == null || skillType == null)
            return false;

        var effect = EffectFactory.CreateEffect(skillData.Projectile);
        if (!effect)
            return false;

        if (effect.gameObject.TryGetComponent<Projectile>(out var projectile))
            projectile = effect.gameObject.AddComponent<Projectile>();

        projectile.Caster = caster;
        projectile.Target = target;
        projectile.skill = skillType;
        projectile.isBuffApplied = isBuffApplied;

        IsSkillReady = false;
        return true;
    }
}
