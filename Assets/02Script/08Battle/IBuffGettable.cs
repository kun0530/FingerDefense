using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffGettable
{
    BuffHandler BuffHandler { get; }
    bool IsBuffGettable { get; }

    bool TakeBuff(BuffData buffData);
    bool TryTakeBuff(BuffData buffData, out Buff buff, bool isTimerStop = false);
}
