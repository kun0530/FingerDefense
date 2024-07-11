using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private MonsterController monster;

    private float patrolTimer = 0f;
    private float patrolInterval = 0.25f;

    private IFindable findBehavior;

    public PatrolState(MonsterController monster, IFindable findBehavior)
    {
        this.monster = monster;
        this.findBehavior = findBehavior;
    }

    public void Enter()
    {
        patrolTimer = 0f;
    }

    public void Update()
    {
        if (monster.moveTarget == null)
        {
            monster.TryTransitionState<IdleState<MonsterController>>();
            return;
        }

        // 성 포탈까지 이동
        var direction = (monster.moveTarget.transform.position - monster.transform.position).normalized;
        monster.transform.position += direction * monster.Status.currentMoveSpeed * Time.deltaTime;
        if (Vector2.Distance(monster.transform.position, monster.moveTarget.transform.position) < 0.1)
        {
            monster.transform.position = monster.moveTarget.transform.position;
            monster.TryTransitionState<IdleState<MonsterController>>();
            return;
        }

        // 타겟 검색
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolInterval)
        {
            patrolTimer = 0f;
            FindTarget();
        }

        if (monster.attackTarget != null)
        {
            monster.TryTransitionState<ChaseState>();
            return;
        }
    }

    public void Exit()
    {
        
    }

    private void FindTarget()
    {
        var nearCollider = findBehavior.FindTarget() as PlayerCharacterController;

        if (nearCollider != null && nearCollider.gameObject != monster.attackTarget)
        {
            monster.attackTarget?.TryRemoveMonster(monster);
            nearCollider.TryAddMonster(monster);
        }

    }
}
