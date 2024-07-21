using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 필요할 때마다 프로퍼티에서 계산한다.
// 2. 변경이 발생할 때마다 필드를 업데이트한다.
public class MonsterStatus : IStatus
{
    public MonsterData data;
    public BuffHandler buffHandler;

    public float currentHp=0;
    public float currentAtk;
    public float currentMoveSpeed;
    public float currentAtkSpeed;

    public MonsterStatus(MonsterData data)
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
        currentAtk = data.AtkDmg;
        currentMoveSpeed = data.MoveSpeed;
        currentAtkSpeed = data.AtkSpeed;
    }

    public void UpdateCurrentState()
    {
        currentHp = data.Hp;
        currentAtk = data.AtkDmg;
        currentMoveSpeed = data.MoveSpeed;
        currentAtkSpeed = data.AtkSpeed;

        foreach (var buff in buffHandler.activeBuffs)
        {
            foreach (var buffAction in buff.BuffActions)
            {
                switch ((BuffType)buffAction.type)
                {
                    case BuffType.ATK_SPEED:
                        currentAtkSpeed += buffAction.value;
                        break;
                    case BuffType.MOVE_SPEED:
                        currentMoveSpeed += buffAction.value;
                        break;
                    case BuffType.HP:
                        break;
                    case BuffType.ATK:
                        currentAtk += buffAction.value;
                        break;
                }
            }
        }

        currentAtk = currentAtk < 0f ? 0f : currentAtk;
        currentMoveSpeed = currentMoveSpeed < 0f ? 0f : currentMoveSpeed;
        currentAtkSpeed = currentAtkSpeed < 0f ? 0f : currentAtkSpeed;
    }
}
