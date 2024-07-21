using System.Collections;
using System.Collections.Generic;
using Defines;
using UnityEngine;

public static class SkillFactory
{
    public static BaseSkill CreateSkill(SkillData data, Transform center)
    {
        LayerMask layerMask = Layers.DEFAULT_LAYER;
        switch (data.Target)
        {
            case 0:
                layerMask = Layers.PLAYER_LAYER;
                break;
            case 1:
                layerMask = Layers.MONSTER_LAYER;
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
                skill = new AreaTargetSkill(findable);
                break;
        }

        if (data.Damage != 0f)
        {
            // skill.skillActions.Add(new AttackSkill(data.Damage));
            skill.attackSkill = new AttackSkill(data.Damage);
        }
        if (data.BuffId != 0)
        {
            var buffTable = DataTableManager.Get<BuffTable>(DataTableIds.Buff);
            var buffData = buffTable.Get(data.BuffId);
            // skill.skillActions.Add(new BuffSkill(buffData));
            skill.buffSkill = new BuffSkill(buffData);
        }

        return skill;
    }
}
