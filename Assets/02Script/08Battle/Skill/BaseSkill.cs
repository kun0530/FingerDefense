using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill
{
    private SkillData skillData;
    protected SkillType skillType;
    protected IFindable targetingMethod;

    private float skillTimer = 0f;
    public bool IsSkillReady { get; protected set; } = false;

    public BaseSkill(SkillData skillData, SkillType skillType, IFindable targetingMethod)
    {
        this.skillData = skillData;
        this.skillType = skillType;
        this.targetingMethod = targetingMethod;
    }

    public void TimerUpdate()
    {
        if (IsSkillReady || skillData == null)
            return;

        skillTimer += Time.deltaTime;
        if (skillTimer >= skillData.CoolTime)
        {
            IsSkillReady = true;
            skillTimer = 0f;
        }
    }

    public abstract void UseSkill();
}
