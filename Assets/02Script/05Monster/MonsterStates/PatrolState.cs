using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class PatrolState : IState
{
    private MonsterController controller;

    private float patrolTimer = 0f;
    private float patrolInterval = 0.25f;

    private IFindable findBehavior;

    private TrackEntry moveTrackEntry;

    public PatrolState(MonsterController controller, IFindable findBehavior)
    {
        this.controller = controller;
        this.findBehavior = findBehavior;
    }

    public void Enter()
    {
        patrolTimer = 0f;

        if (controller.monsterAni.CurrentMonsterState != MonsterSpineAni.MonsterState.WALK)
            moveTrackEntry = controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.WALK, true, controller.Status.CurrentMoveSpeed);
        else
            moveTrackEntry = controller.monsterAni.CurrentTrackEntry;
    }

    public void Update()
    {
        if (controller.moveTargetPos == null)
        {
            controller.TryTransitionState<IdleState<MonsterController>>();
            return;
        }

        // 성 포탈까지 이동
        var direction = (controller.moveTargetPos - controller.transform.position).normalized;
        direction *= controller.directionMultiplier;
        controller.transform.position += direction * controller.Status.CurrentMoveSpeed * Time.deltaTime;
        if (moveTrackEntry != null)
            moveTrackEntry.TimeScale = controller.Status.CurrentMoveSpeed;
        controller.SetFlip(direction.x > 0);
        if (Vector2.Distance(controller.transform.position, controller.moveTargetPos) < 0.1)
        {
            controller.transform.position = controller.moveTargetPos;
            controller.TryTransitionState<AttackCastleState>();
            return;
        }

        // 타겟 검색
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolInterval)
        {
            patrolTimer = 0f;
            FindTarget();
        }

        if (controller.attackTarget != null)
        {
            controller.TryTransitionState<ChaseState>();
            return;
        }
    }

    public void Exit()
    {
    }

    private void FindTarget()
    {
        var nearCollider = findBehavior.FindTarget();
        if (nearCollider == null)
            return;
        
        if (nearCollider.TryGetComponent<PlayerCharacterController>(out var target)
        && target != controller.attackTarget)
        {
            controller.attackTarget?.TryRemoveMonster(controller);
            target.TryAddMonster(controller);
        }
    }
}
