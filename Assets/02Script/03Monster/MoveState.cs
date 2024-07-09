using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState
{
    private MonsterController controller;
    private Vector3 direction = new Vector3(1f, 0f, 0f);

    public MoveState(MonsterController controller)
    {
        this.controller = controller;
    }

    public MoveState(MonsterController controller, Vector3 dir)
    {
        this.controller = controller;
        direction = dir;
    }

    public void Enter()
    {
    }
    
    public void Update()
    {
        if (controller.moveTarget == null)
            return;

        if (controller.attackTarget == null)
        {
            var dir = (controller.moveTarget.position - controller.transform.position).normalized;
            controller.transform.position += dir * controller.Data.MoveSpeed * Time.deltaTime;
        }
        else if (Vector2.Distance(controller.transform.position, controller.attackMoveTarget.position) > 0.1)
        {
            var dir = controller.attackMoveTarget.position - controller.transform.position;
            dir.Normalize();

            controller.transform.position += dir * controller.Data.MoveSpeed * Time.deltaTime;
        }
        else
        {
            controller.transform.position = controller.attackMoveTarget.position;
        }

        if (controller.CanPatrol)
            controller.TryTransitionState<PatrolState>();
    }

    public void Exit()
    {
    }
}
