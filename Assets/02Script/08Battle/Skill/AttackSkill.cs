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

    public void ApplySkillAction(IDamageable damageable)
    {
        damageable.TakeDamage(damage);
    }
}
