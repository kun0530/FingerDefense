using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : ISkillAction
{
    // 버프 데이터 저장
    public BuffData buffData { get; private set; }

    public BuffSkill(BuffData data) // 버프 데이터 받음
    {
        buffData = data;
    }

    public bool ApplySkillAction(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            return damageable.TakeBuff(buffData);
        }
        else
        {
            return false;
        }
    }

    public Buff ApplySkillEnterAreaAction(IDamageable damageable)
    {
        var buff = new Buff(buffData, true);
        damageable.TakeBuff(buff);

        return buff;
    }
}