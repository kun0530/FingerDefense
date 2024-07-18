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

    public void ApplySkillAction(IDamageable damageable)
    {
        // 버프 및 디버프 부여
        damageable.TakeBuff(buffData);
    }

    public Buff ApplySkillEnterAreaAction(IDamageable damageable)
    {
        var buff = new Buff(buffData, true);
        damageable.TakeBuff(buff);

        return buff;
    }
}