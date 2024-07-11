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
        controller.transform.position += direction * controller.Status.currentMoveSpeed * Time.deltaTime;

        if (controller.CanPatrol)
            controller.TryTransitionState<PatrolState>();
    }

    public void Exit()
    {
    }
}
