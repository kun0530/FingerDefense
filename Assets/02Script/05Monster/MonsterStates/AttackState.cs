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
        attackCoolDown = 1f / monster.Status.data.AtkSpeed;
        attackTimer = attackCoolDown;
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
            monster.attackTarget.DamageHp(monster.Status.currentAtk);
            attackTimer = 0f;

            // monster.transform.position = 
            return;
        }

        // 일단은 공격 모션 To-Do: 공격 애니메이션 받으면 삭제
        if (attackTimer <= attackCoolDown / 2f) // attack target으로 이동
        {
            monster.transform.position = Vector2.Lerp(monster.transform.position, monster.attackTarget.transform.position, attackTimer / (attackCoolDown / 2f));
        }
        else // attack move target으로 이동
        {
            monster.transform.position = Vector2.Lerp(monster.transform.position, monster.attackMoveTarget.transform.position, attackTimer / (attackCoolDown / 2f));
        }
    }

    public void Exit()
    {
    }
}
