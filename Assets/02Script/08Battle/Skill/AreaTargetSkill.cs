using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTargetSkill : BaseSkill
{
    IFindable findBehavior;
    private SkillArea skillArea;

    // public AreaTargetSkill(AreaSkill area)
    // {
    //     this.area = area;
    // }

    public override void UseSkill()
    {
        var damageable = findBehavior.FindTarget();
        var area = GameObject.Instantiate(skillArea);
        area.transform.position = damageable.transform.position;
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
