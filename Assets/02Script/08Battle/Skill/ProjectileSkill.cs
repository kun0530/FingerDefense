using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : BaseSkill
{
    // 투사체 프리팹

    public ProjectileSkill(SkillType baseSkill, IFindable targetingMethod)
    : base(baseSkill, targetingMethod) { }


    public override void UseSkill()
    {
        // 투사체 프리팹 생성
        // 투사체 프리팹에 BaseSkill 레퍼런스 연결
    }
}
