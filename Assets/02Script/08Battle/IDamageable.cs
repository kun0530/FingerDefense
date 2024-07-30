using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    bool TakeDamage(float damage);
    // void TakeDamage(float hp, 공격자)

    bool TakeBuff(BuffData buffData);
    bool TakeBuff(Buff buff);
}