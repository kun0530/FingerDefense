using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : ISkillAction
{
    private float damage;
    private BuffHandler buffHandler;

    public AttackSkill(float damage, GameObject gameObject = null)
    {
        this.damage = damage;
        if (gameObject != null && gameObject.TryGetComponent<IBuffGettable>(out var buffGettable))
        {
            buffHandler = buffGettable.BuffHandler;
        }
    }

    public bool ApplySkillAction(GameObject target, bool isBuffApplied = false)
    {
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            if (isBuffApplied && buffHandler != null)
                return damageable.TakeDamage(damage + buffHandler.buffValues[BuffType.ATK]);
                
            return damageable.TakeDamage(damage);
        }

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