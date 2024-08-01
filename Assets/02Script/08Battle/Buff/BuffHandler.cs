using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler
{
    public List<Buff> buffs = new();
    public Dictionary<BuffType, float> buffValues = new();
    public event Func<float, bool> OnDotDamage;

    public int maxBuffCount = 3;

    public BuffHandler()
    {
        for (int i = 0; i < (int)BuffType.COUNT; i++)
        {
            buffValues.Add((BuffType)i, 0f);
        }
    }

    public void ResetBuffs()
    {
        buffs.Clear();
        UpdateBuff();
    }

    public void TimerUpdate()
    {
        Stack<int> removeIndexes = new();
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].TimerUpdate();
            if (buffs[i].IsBuffExpired)
            {
                removeIndexes.Push(i);
            }
            if (buffs[i].isDotDamage)
            {
                OnDotDamage?.Invoke(buffs[i].dotDamage);
                buffs[i].isDotDamage = false;
            }
        }

        while (removeIndexes.Count != 0)
        {
            RemoveBuff(removeIndexes.Pop());
        }
    }

    public bool AddBuff(BuffData data)
    {
        if (data == null)
        {
            Logger.LogError("해당 버프의 정보가 없습니다.");
            return false;
        }

        var buff = new Buff(data);
        return AddBuff(buff);
    }

    public bool AddBuff(Buff buff)
    {
        if (buff == null)
        {
            Logger.LogError("해당 버프의 정보가 없습니다.");
            return false;
        }

        if (buffs.Count >= maxBuffCount)
            return false;

        buffs.Add(buff);
        UpdateBuff();

        return true;
    }

    private void RemoveBuff(int index)
    {
        if (index < 0 || index >= buffs.Count)
            return;

        buffs.RemoveAt(index);
        UpdateBuff();
    }

    public void UpdateBuff()
    {
        for (int i = 0; i < (int)BuffType.COUNT; i++)
        {
            buffValues[(BuffType)i] = 0f;
        }

        foreach (var buff in buffs)
        {
            foreach (var buffAction in buff.buffData.BuffActions)
            {
                buffValues[(BuffType)buffAction.type] += buffAction.value;
            }
        }
    }
}