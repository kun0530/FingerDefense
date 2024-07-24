using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTargetSkill : SkillType
{
    private SkillArea skillArea;

    public AreaTargetSkill(IFindable secondaryTargeting, string assetId)
    : base(secondaryTargeting, assetId) { }


    // public AreaTargetSkill(AreaSkill area)
    // {
    //     this.area = area;
    // }

    public override void UseSkill(GameObject target)
    {
        // var area = GameObject.Instantiate(skillArea);
        // area.transform.position = target.transform.position;

        Logger.Log("미구현된 스킬입니다: 설치 스킬");
        return;
    }

    public void EnterArea(IDamageable damageable, SkillArea area)
    {
        if (buffSkill != null)
        {
            var buff = buffSkill.ApplySkillEnterAreaAction(damageable);
            area.Buffs.Add(damageable, buff);
        }

        if (attackSkill != null)
        {
            attackSkill.ApplySkillAction(damageable);
        }
    }

    public void ExitArea(IDamageable damageable, SkillArea area)
    {
        if (buffSkill != null && area.Buffs.TryGetValue(damageable, out var buff))
        {
            buff.IsTimerStop = true;
            area.Buffs.Remove(damageable);
        }
    }
}
