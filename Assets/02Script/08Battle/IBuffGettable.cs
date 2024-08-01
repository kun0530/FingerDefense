using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffGettable
{
    bool IsBuffGettable { get; }

    bool TakeBuff(BuffData buffData);
    bool TakeBuff(Buff buff);
}
