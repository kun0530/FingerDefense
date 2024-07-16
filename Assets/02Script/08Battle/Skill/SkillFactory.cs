using System.Collections;
using System.Collections.Generic;
using Defines;
using UnityEngine;

public static class SkillFactory
{
    public static BaseSkill CreateSkill(SkillData data, Transform center)
    {
        LayerMask layerMask = Laysers.DEFAULT_LAYER;
        switch (data.Target)
        {
            case 0:
                layerMask = Laysers.PLAYER_LAYER;
                break;
            case 1:
                layerMask = Laysers.MONSTER_LAYER;
                break;
        }
        IFindable findable = new FindingTargetInCircle(center, data.RangeValue, layerMask);

        BaseSkill skill = null;

        switch ((SkillRangeTypes)data.RangeType)
        {
            case SkillRangeTypes.SingleTarget:
                skill = new SingleTargetSkill(findable);
                break;
            case SkillRangeTypes.MultipleTarget:
                skill = new MultipleTargetSkill(findable);
                break;
            case SkillRangeTypes.AreaTarget:
                skill = new AreaTargetSkill();
                break;
        }

        if (data.Damage != 0f)
        {
            skill.skillActions.Add(new AttackSkill(data.Damage));
        }
        if (data.BuffId != 0)
        {
            skill.skillActions.Add(new BuffSkill());
        }

        return skill;
    }
}
