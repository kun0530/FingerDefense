using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus
{
    public PlayerCharacterData data;

    public float currentHp;
    public float currentAtkDmg;
    public float currnetAtkSpeed;

    public CharacterStatus(PlayerCharacterData data)
    {
        this.data = data;
        Init();
    }

    private void Init()
    {
        if (data == null)
            return;

        currentHp = data.Hp;
        currentAtkDmg = data.AtkDmg;
        currnetAtkSpeed = data.AtkSpeed;
    }
}