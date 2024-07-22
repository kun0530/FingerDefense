using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetSkill : SkillType
{
    public override void UseSkill(GameObject target)
    {
        if (target != null && target.TryGetComponent<IDamageable>(out var damageable))
        {
            ApplySkillActions(damageable);
        }
    }
}
