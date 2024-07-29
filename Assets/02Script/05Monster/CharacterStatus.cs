using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : BaseStatus
{
    private PlayerCharacterData data;
    public PlayerCharacterData Data
    {
        get => data;
        set
        {
            data = value;
            Init();
        }
    }

    public override void Init()
    {
        if (Data == null)
            return;

        currentMaxHp = Data.Hp;
        CurrentHp = currentMaxHp;
    }
}