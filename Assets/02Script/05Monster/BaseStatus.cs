using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatus
{
    public BuffHandler buffHandler;

    public event Action OnHpBarUpdate;
    public float maxHp;
    protected float currentHp;
    public float CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = value;
            OnHpBarUpdate?.Invoke();
        }
    }

    public float currentAtkDmg;
    public float currentAtkSpeed;

    public abstract void Init();
    public abstract void UpdateCurrentState();
}