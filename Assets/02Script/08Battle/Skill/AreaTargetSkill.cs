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

    public void EnterArea(IDamageable damageable)
    {

    }

    public void ExitArea(IDamageable damageable)
    {
        
    }
}
