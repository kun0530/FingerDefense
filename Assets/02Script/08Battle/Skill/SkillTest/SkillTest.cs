using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTest : MonoBehaviour
{
    public TMP_InputField skillId;
    public SkillTable skillTable;

    public PlayerAttackBehavior playerAttackBehavior;

    private void Awake()
    {
        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
    }

    public void CreateSkill()
    {
        if (int.TryParse(skillId.text, out var id))
        {
            var skillData = skillTable.Get(id);
            if (skillData == null)
            {
                Logger.Log("유효하지 않는 스킬 아이디입니다.");
                return;
            }
            var skill = SkillFactory.CreateSkill(skillData, playerAttackBehavior.gameObject);
            playerAttackBehavior.baseSkill = skill;
        }
        else
        {
            Logger.Log("int값이 아닙니다.");
            return;
        }
    }
}
