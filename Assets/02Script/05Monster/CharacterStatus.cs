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

        CurrentHp = Data.Hp;
        maxHp = Data.Hp;
        // currentAtkDmg = Data.AtkDmg;
        // currentAtkSpeed = Data.AtkSpeed;
    }

    public override void UpdateCurrentState()
    {
        if(Data == null)
        {
            return;
        }
        
        // currentAtkDmg = Data.AtkDmg;
        // currentAtkSpeed = Data.AtkSpeed;

        foreach (var buff in buffHandler.buffs)
        {
            foreach (var buffAction in buff.buffData.BuffActions)
            {
                switch ((BuffType)buffAction.type)
                {
                    case BuffType.ATK_SPEED:
                        currentAtkSpeed += buffAction.value;
                        break;
                    case BuffType.ATK:
                        currentAtkDmg += buffAction.value;
                        break;
                }
            }
        }

        currentAtkDmg = currentAtkDmg < 0f ? 0f : currentAtkDmg;
        currentAtkSpeed = currentAtkSpeed < 0f ? 0f : currentAtkSpeed;

        // Logger.Log($"기본 공격력: {Data.AtkDmg} / 현재 공격력: {currentAtkDmg}");
    }
}