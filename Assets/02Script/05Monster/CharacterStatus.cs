using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus
{
    public PlayerCharacterData data;
    public BuffHandler buffHandler;

    public float currentHp;
    public float currentAtkDmg;
    public float currnetAtkSpeed;

    public CharacterStatus(PlayerCharacterData data)
    {
        this.data = data;
        buffHandler = new(this);
        Init();
    }

    public void Init()
    {
        if (data == null)
            return;

        currentHp = data.Hp;
        currentAtkDmg = data.AtkDmg;
        currnetAtkSpeed = data.AtkSpeed;
    }

    public void UpdateCurrentState()
    {
        currentAtkDmg = data.AtkDmg;
        currnetAtkSpeed = data.AtkSpeed;

        foreach (var buff in buffHandler.activeBuffs)
        {
            foreach (var buffAction in buff.BuffActions)
            {
                switch ((BuffType)buffAction.type)
                {
                    case BuffType.ATK_SPEED:
                        currnetAtkSpeed += buffAction.value;
                        break;
                    case BuffType.ATK:
                        currentAtkDmg += buffAction.value;
                        break;
                }
            }
        }

        currentAtkDmg = currentAtkDmg < 0f ? 0f : currentAtkDmg;
        currnetAtkSpeed = currnetAtkSpeed < 0f ? 0f : currnetAtkSpeed;

        Logger.Log($"기본 공격력: {data.AtkDmg} / 현재 공격력: {currentAtkDmg}");
    }
}