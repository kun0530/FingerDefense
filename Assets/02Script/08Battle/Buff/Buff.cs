using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public BuffData buffData { get; private set; }
    private float timer;
    public bool IsTimerStop { get; set; }
    public bool IsBuffExpired { get; private set; }
    // public IDamageable damageable;

    public Buff(BuffData data, bool isTimerStop = false)
    {
        buffData = data;
        timer = 0f;
        this.IsTimerStop = isTimerStop;
        IsBuffExpired = false;
    }

    public void TimerUpdate()
    {
        if (IsTimerStop)
            return;

        timer += Time.deltaTime;

        if (timer >= buffData.LastingTime)
            IsBuffExpired = true;
    }

    public override string ToString()
    {
        return buffData.Id.ToString();
    }
}