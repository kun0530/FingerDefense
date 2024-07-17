using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float hp);
    // void TakeDamage(float hp, 공격자)

    void TakeBuff(BuffData buffData);
}