using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler
{
    public List<BuffData> activeBuffs = new();
    private List<float> timers = new();

    private CharacterStatus status;

    public BuffHandler(CharacterStatus status)
    {
        this.status = status;
    }

    private void ResetBuff()
    {
        activeBuffs.Clear();
        timers.Clear();
    }

    public void TimerUpdate()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            timers[i] += Time.deltaTime;
            if (timers[i] >= activeBuffs[i].LastingTime)
            {
                RemoveBuff(i);
            }
        }
    }

    public void AddBuff(BuffData data)
    {
        if (activeBuffs.Count >= 3)
        {
            Logger.Log($"Buff 최대({activeBuffs.Count}): {data.Id}");
            return;
        }

        activeBuffs.Add(data);
        timers.Add(0f);
        status.UpdateCurrentState();

        Logger.Log($"Buff 추가: {data.Id}");
    }

    private void RemoveBuff(int index)
    {
        if (index < 0 || index >= activeBuffs.Count)
            return;

        Logger.Log($"Buff 제거: {activeBuffs[index].Id}");

        activeBuffs.RemoveAt(index);
        timers.RemoveAt(index);
        status.UpdateCurrentState();
    }
}