using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill
{
    protected SkillType baseSkill;
    protected IFindable targetingMethod;

    public BaseSkill(SkillType baseSkill, IFindable targetingMethod)
    {
        this.baseSkill = baseSkill;
        this.targetingMethod = targetingMethod;
    }

    public abstract void UseSkill();
    // protected List<GameObject> FindTargets => targetingMethod.FindTargets();
}
