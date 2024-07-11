using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 필요할 때마다 프로퍼티에서 계산한다.
// 2. 변경이 발생할 때마다 필드를 업데이트한다.
public class MonsterStatus
{
    public MonsterData data;

    // + 버프, 디버프 컬렉션

    public float currentHp;
    public float currentAtk;
    public float currentMoveSpeed;
    public float currentAtkSpeed;

    public MonsterStatus(MonsterData data)
    {
        this.data = data;
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
}
