using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private MonsterController controller;

    public ChaseState(MonsterController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.WALK, true, controller.Status.currentMoveSpeed);
    }

    public void Update()
    {
        if (controller.attackTarget == null)
        {
            controller.TryTransitionState<PatrolState>();
            return;
        }

        var direction = (controller.attackMoveTarget.position - controller.transform.position).normalized;
        controller.transform.position += direction * controller.Status.currentMoveSpeed * Time.deltaTime;
        controller.SetFlip(direction.x > 0);
        if (Vector2.Distance(controller.transform.position, controller.attackMoveTarget.position) < 0.1)
        {
            controller.transform.position = controller.attackMoveTarget.position;
            controller.TryTransitionState<AttackState>();
            return;
        }
    }

    public void Exit()
    {
    }
}
