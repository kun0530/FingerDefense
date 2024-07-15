using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill
{
    public List<ISkillAction> skillActions = new List<ISkillAction>();

    public abstract void UseSkill();

    protected void ApplySkillActions(IDamageable damageable)
    {
        foreach (var skillAction in skillActions)
        {
            skillAction.ApplySkillAction(damageable);
        }
    }
}