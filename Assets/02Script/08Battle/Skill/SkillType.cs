using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillRangeTypes
{
    SingleTarget, MultipleTarget, AreaTarget
}

public abstract class SkillType
{
    // public List<ISkillAction> skillActions = new List<ISkillAction>();
    public BuffSkill buffSkill;
    public AttackSkill attackSkill;
    protected IFindable secondaryTargeting;

    public string AssetId { get; protected set; }

    public SkillType(IFindable secondaryTargeting, SkillData data)
    {
        this.secondaryTargeting = secondaryTargeting;
        this.AssetId = data.AssetNo;
    }

    public abstract bool UseSkill(GameObject target);

    protected bool ApplySkillActions(GameObject target)
    {
        var isAttacked = attackSkill != null ? attackSkill.ApplySkillAction(target) : false;
        var isBuffed = buffSkill != null ? buffSkill.ApplySkillAction(target) : false;

        return isAttacked || isBuffed;
    }
}