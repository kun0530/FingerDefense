using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatus
{
    public BuffHandler buffHandler;

    public event Action OnHpBarUpdate;
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

    public BaseStatus(BuffHandler buffHandler)
    {
        this.buffHandler = buffHandler;
    }

    public abstract void Init();
    public abstract void UpdateCurrentState();
}