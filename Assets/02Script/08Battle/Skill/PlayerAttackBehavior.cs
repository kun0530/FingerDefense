using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour
{
    public BaseSkill normalAttack;
    public BaseSkill skillAttack;

    private Queue<BaseSkill> readySkills = new();
    private BaseSkill currentAttack;

    private void Update()
    {
        UpdateSkill(normalAttack);
        UpdateSkill(skillAttack);

        UseReadySkill();
    }

    private void UpdateSkill(BaseSkill skill)
    {
        if (skill == null || skill.IsSkillReady)
            return;

        skill.TimerUpdate();

        if (skill.IsSkillReady)
        {
            readySkills.Enqueue(skill);
        }
    }

    private void UseReadySkill()
    {
        if (currentAttack != null && !currentAttack.IsSkillCompleted)
            return;

        if (readySkills.Count != 0)
        {
            currentAttack = readySkills.Dequeue();
            currentAttack.UseSkill();
        }
    }
}