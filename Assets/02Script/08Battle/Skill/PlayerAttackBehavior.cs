using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    private BaseSkill skill;
    private SkillData skillData;
    private float skillTimer = 0f;

    public Skill(SkillData data, PlayerAttackBehavior attackBehavior)
    {
        skillData = data;
        skill = SkillFactory.CreateSkill(skillData, attackBehavior.gameObject);
    }

    public void TimerUpdate()
    {
        if (skill == null || skillData == null)
            return;

        skillTimer += Time.deltaTime;
        if (skillTimer >= skillData.CoolTime)
        {
            skill.UseSkill();
            skillTimer = 0f;
        }
    }
}

public class PlayerAttackBehavior : MonoBehaviour
{
    private Skill skill;

    private void Update()
    {
        skill?.TimerUpdate();
    }
}