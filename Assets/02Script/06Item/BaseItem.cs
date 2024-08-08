using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    public virtual bool IsPassive { get => false; }

    public abstract bool UseItem();
    public virtual bool CancelItem() { return true; }
    public virtual void UpdateItem() { }
}