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
        {
            return damageable.TakeDamage(damage);
        }
        else
        {
            return false;
        }
    }
}