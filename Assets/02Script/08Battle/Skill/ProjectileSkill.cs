using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : BaseSkill
{
    // 투사체 프리팹

    public ProjectileSkill(SkillData skillData, SkillType skillType, IFindable targetingMethod)
    : base(skillData, skillType, targetingMethod) { }

    public override bool UseSkill()
    {
        // 투사체 프리팹 생성
        // 투사체 프리팹에 BaseSkill 레퍼런스 연결
        return true;
    }
}
