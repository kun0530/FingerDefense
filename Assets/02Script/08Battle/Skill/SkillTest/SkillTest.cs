using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTest : MonoBehaviour
{
    public TMP_InputField skillId;
    public SkillTable skillTable;

    public PlayerAttackBehavior playerAttackBehavior;
    public MonsterSpawnTest monsterSpawnTest;

    private void Awake()
    {
        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
    }

    public void ApplySkillDataToPlayer()
    {
        var skill = CreateSkill();
        playerAttackBehavior.baseSkill = skill;
    }

    public void ApplySkillDataToMonster()
    {
        if (int.TryParse(skillId.text, out var id))
        {
            monsterSpawnTest.monsterData.Skill = id;
        }
    }

    public BaseSkill CreateSkill()
    {
        if (int.TryParse(skillId.text, out var id))
        {
            var skillData = skillTable.Get(id);
            if (skillData == null)
            {
                Logger.Log("유효하지 않는 스킬 아이디입니다.");
                return null;
            }
            var skill = SkillFactory.CreateSkill(skillData, playerAttackBehavior.gameObject);
            return skill;
        }
        else
        {
            Logger.Log("int값이 아닙니다.");
            return null;
        }
    }
}
