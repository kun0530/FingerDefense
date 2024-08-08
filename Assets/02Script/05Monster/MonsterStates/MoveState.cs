using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class MoveState : IState
{
    private MonsterController controller;
    private Vector3 direction = new Vector3(-1f, 0f, 0f);

    private TrackEntry moveTrackEntry;

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
        direction = Vector3.zero;

        if (controller.monsterAni.CurrentMonsterState != MonsterSpineAni.MonsterState.WALK)
            moveTrackEntry = controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.WALK, true, controller.Status.CurrentMoveSpeed);
        else
            moveTrackEntry = controller.monsterAni.CurrentTrackEntry;
    }
    
    public void Update()
    {
        if (controller.moveTarget != null)
        {
            direction.x = controller.moveTarget.transform.position.x
            > controller.transform.position.x ? 1f : -1f;
            direction.x *= controller.directionMultiplier;
        }
        controller.SetFlip(direction.x > 0);

        controller.transform.position += direction * controller.Status.CurrentMoveSpeed * Time.deltaTime;
        if (moveTrackEntry != null)
            moveTrackEntry.TimeScale = controller.Status.CurrentMoveSpeed;

        if (controller.CanPatrol)
            controller.TryTransitionState<PatrolState>();
    }

    public void Exit()
    {
    }
}
