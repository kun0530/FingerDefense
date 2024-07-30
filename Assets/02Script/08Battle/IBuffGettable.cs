using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffGettable
{
    bool TakeBuff(BuffData buffData);
    bool TakeBuff(Buff buff);
}
