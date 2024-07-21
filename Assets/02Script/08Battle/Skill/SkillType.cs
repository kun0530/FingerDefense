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

    public abstract void UseSkill(GameObject target);

    protected void ApplySkillActions(IDamageable damageable)
    {
        attackSkill?.ApplySkillAction(damageable);
        buffSkill?.ApplySkillAction(damageable);
    }
}