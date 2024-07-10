using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private MonsterController monster;

    private float attackTimer;
    private float attackCoolDown;

    public AttackState(MonsterController monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        Logger.Log("!");

        attackTimer = 0f;
        attackCoolDown = monster.status.data.AtkSpeed / 60f;
    }

    public void Update()
    {
        if (monster.attackTarget == null) // 그 외 추가 조건(공격 중단) 확인바람
        {
            monster.TryTransitionState<PatrolState>();
            return;
        }

        if (Vector2.Distance(monster.transform.position, monster.attackMoveTarget.position) > 0.1) // Attack Move Target의 위치 변경
        {
            monster.TryTransitionState<ChaseState>();
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCoolDown)
        {
            Logger.Log("공격!");
        }
    }

    public void Exit()
    {
    }
}
