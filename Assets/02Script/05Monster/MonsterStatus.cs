using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 필요할 때마다 프로퍼티에서 계산한다.
// 2. 변경이 발생할 때마다 필드를 업데이트한다.
public class MonsterStatus : BaseStatus
{
    private MonsterData data;
    public MonsterData Data
    {
        get => data;
        set
        {
            data = value;
            Init();
        }
    }

    public float currentMoveSpeed;

    public MonsterStatus(BuffHandler buffHandler) : base(buffHandler) { }

    public override void Init()
    {
        if (Data == null)
            return;

        CurrentHp = Data.Hp;
        currentAtkDmg = Data.AtkDmg;
        currentMoveSpeed = Data.MoveSpeed;
        currentAtkSpeed = Data.AtkSpeed;
    }

    public override void UpdateCurrentState()
    {
        CurrentHp = Data.Hp;
        currentAtkDmg = Data.AtkDmg;
        currentMoveSpeed = Data.MoveSpeed;
        currentAtkSpeed = Data.AtkSpeed;

        foreach (var buff in buffHandler.buffs)
        {
            foreach (var buffAction in buff.buffData.BuffActions)
            {
                switch ((BuffType)buffAction.type)
                {
                    case BuffType.ATK_SPEED:
                        currentAtkSpeed += buffAction.value;
                        break;
                    case BuffType.MOVE_SPEED:
                        currentMoveSpeed += buffAction.value;
                        break;
                    case BuffType.DOT_HP:
                        break;
                    case BuffType.ATK:
                        currentAtkDmg += buffAction.value;
                        break;
                }
            }
        }

        currentAtkDmg = currentAtkDmg < 0f ? 0f : currentAtkDmg;
        currentMoveSpeed = currentMoveSpeed < 0f ? 0f : currentMoveSpeed;
        currentAtkSpeed = currentAtkSpeed < 0f ? 0f : currentAtkSpeed;
    }
}
