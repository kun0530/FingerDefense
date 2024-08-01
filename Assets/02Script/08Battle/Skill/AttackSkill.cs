using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : ISkillAction
{
    private float damage;

    public AttackSkill(float damage)
    {
        this.damage = damage;
    }

    public bool ApplySkillAction(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out var damageable))
            return damageable.TakeDamage(damage);

        return false;
    }

    public bool EnterSkillArea(GameObject target, SkillArea area)
    {
        return ApplySkillAction(target);
    }

    public bool ExitSkillArea(GameObject target, SkillArea area)
    {
        return true;
    }
}