using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler
{
    public List<Buff> buffs = new();

    private IStatus status;

    public BuffHandler(IStatus status)
    {
        this.status = status;
    }

    private void ResetBuff()
    {
        // activeBuffs.Clear();
        // timers.Clear();

        buffs.Clear();
    }

    public void TimerUpdate()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].TimerUpdate();
            if (buffs[i].IsBuffExpired)
            {
                RemoveBuff(i);
            }
        }
    }

    public void AddBuff(BuffData data)
    {
        var buff = new Buff(data);
    }

    public void AddBuff(Buff buff)
    {
        if (buffs.Count >= 3)
        {
            Logger.Log($"Buff 최대({buffs.Count}): {buff.ToString()}");
            return;
        }

        buffs.Add(buff);
        status.UpdateCurrentState();

        Logger.Log($"Buff 추가: {buff.ToString()}");
    }

    private void RemoveBuff(int index)
    {
        if (index < 0 || index >= buffs.Count)
            return;

        Logger.Log($"Buff 제거: {buffs[index].ToString()}");

        buffs.RemoveAt(index);
        status.UpdateCurrentState();
    }
}