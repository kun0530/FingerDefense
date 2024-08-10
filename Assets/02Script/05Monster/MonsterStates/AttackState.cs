using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class AttackState : IState
{
    private MonsterController controller;

    private float attackTimer;
    private float attackCoolDown;

    private TrackEntry attackTrackEntry;

    public AttackState(MonsterController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        attackCoolDown = 1f / controller.Status.Data.AtkSpeed;
        attackTimer = attackCoolDown;

        controller.SetFlip(false);

        attackTrackEntry = controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.ATTACK, true, controller.Status.CurrentAtkSpeed);
    }

    public void Update()
    {
        if (!controller.attackTarget) // 그 외 추가 조건(공격 중단) 확인바람
        {
            controller.TryTransitionState<PatrolState>();
            return;
        }

        if (controller.attackTarget.IsDead)
        {
            controller.attackTarget.TryRemoveMonster(controller);
            controller.TryTransitionState<PatrolState>();
            return;
        }

        if (Vector2.Distance(controller.transform.position, controller.attackMoveTarget.position) > 0.1) // Attack Move Target의 위치 변경
        {
            controller.TryTransitionState<ChaseState>();
            return;
        }

        if (attackTrackEntry != null)
            attackTrackEntry.TimeScale = controller.Status.CurrentAtkSpeed;

        if (Mathf.Approximately(controller.Status.Data.AtkSpeed, 0f))
            return;
        
        attackTimer += Time.deltaTime;
        attackCoolDown = 1f / controller.Status.Data.AtkSpeed;
        if (attackTimer >= attackCoolDown)
        {
            controller.attackTarget.TakeDamage(controller.Status.CurrentAtk, DamageReason.MONSTER_HIT_DAMAGE, controller.Status.element);
            attackTimer = 0f;

            return;
        }
    }

    public void Exit()
    {
        controller.attackTarget?.TryRemoveMonster(controller);
    }
}
