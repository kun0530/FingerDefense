using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private MonsterController monster;

    public ChaseState(MonsterController monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        if (monster.attackTarget == null)
        {
            monster.TryTransitionState<PatrolState>();
            return;
        }

        var direction = (monster.attackMoveTarget.position - monster.transform.position).normalized;
        monster.transform.position += direction * monster.Status.data.MoveSpeed * Time.deltaTime;
        if (Vector2.Distance(monster.transform.position, monster.attackMoveTarget.position) < 0.1)
        {
            monster.transform.position = monster.attackMoveTarget.position;
            monster.TryTransitionState<AttackState>();
            return;
        }
    }

    public void Exit()
    {
        
    }
}
