using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private MonsterController monster;

    private float patrolTimer = 0f;
    private float patrolInterval = 0.25f;

    public PatrolState(MonsterController monster)
    {
        this.monster = monster;
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
        monster.transform.position += direction * monster.Data.MoveSpeed * Time.deltaTime;
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
        LayerMask targetLayer = 1 << LayerMask.NameToLayer("Player");
        var targets = Physics2D.OverlapCircleAll(monster.transform.position, 1f, targetLayer);
        PlayerCharacterController nearCollider = null;
        float nearDistance = float.MaxValue;
        foreach (var target in targets)
        {
            if (target.TryGetComponent<PlayerCharacterController>(out var playerCharacter))
            {
                if (playerCharacter.MonsterCount == 2)
                    continue;
                    
                float distance = Vector2.Distance(playerCharacter.transform.position, monster.transform.position);
                if (distance < nearDistance)
                {
                    nearCollider = playerCharacter;
                    nearDistance = distance;
                }
            }
 
        }

        if (nearCollider != null && nearCollider.gameObject != monster.attackTarget)
        {
            monster.attackTarget?.TryRemoveMonster(monster);
            nearCollider.TryAddMonster(monster);
        }

    }
}
