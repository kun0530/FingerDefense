using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState<T> : IState where T : MonsterController
{
    private T controller;
    private float moveSpeed = 1f;
    private Vector3 direction = new Vector3(1f, 0f, 0f);

    private float patrolTimer = 0f;
    private float patrolInterval = 0.25f;
    // private GameObject target;

    public MoveState(T controller)
    {
        this.controller = controller;
    }

    public MoveState(T controller, Vector3 dir)
    {
        this.controller = controller;
        direction = dir;
    }

    public MoveState(T controller, GameObject target)
    {
        this.controller = controller;
        // this.target = target;
    }

    public void Enter()
    {
    }
    
    public void Update()
    {
        if (controller.moveTarget == null)
            return;

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolInterval)
        {
            patrolTimer = 0f;

            var colliders = Physics2D.OverlapCircleAll(controller.transform.position, controller.Data.AtkRange);
            PlayerCharacterController nearCollider = null;
            float nearDistance = float.MaxValue;
            foreach (var col in colliders)
            {
                if (col.TryGetComponent<PlayerCharacterController>(out var playerCharacter))
                {
                    // var canAttack = false;
                    // foreach (var monster in playerCharacter.monsters)
                    // {
                    //     if (monster == controller)
                    //     {   
                    //         canAttack = true;
                    //         break;
                    //     }

                    //     if (monster == null)
                    //         canAttack = true;
                    // }
                    // if (!canAttack)
                    //     continue;
                    if (playerCharacter.monsterCount == 2)
                        break;

                    float distance = Vector2.Distance(playerCharacter.transform.position, controller.transform.position);
                    if (distance < nearDistance)
                    {
                        nearCollider = playerCharacter;
                        nearDistance = distance;
                    }
                }
            }

            if (nearCollider != null && nearCollider.gameObject != controller.attackTarget)
            {
                // if (target != null && target.TryGetComponent<PlayerCharacterController>(out var player))
                // {
                //     for (int i = 0; i < player.monsters.Length; i++)
                //     {
                //         if (player.monsters[i] == controller)
                //         {
                //             player.monsters[i] = null;
                //         }
                //     }
                // }
                nearCollider.TryAddMonster(controller);

                // target = nearCollider.gameObject;
                // attack state로 변경
            }
        }

        if (controller.attackTarget == null)
        {
            var dir = (controller.moveTarget.position - controller.transform.position).normalized;
            controller.transform.position += dir * moveSpeed * Time.deltaTime;

            // 주변 target 감지
        }
        else if (Vector2.Distance(controller.transform.position, controller.attackMoveTarget.position) > 0.1)
        {
            var dir = controller.attackMoveTarget.position - controller.transform.position;
            dir.Normalize();

            controller.transform.position += dir * moveSpeed * Time.deltaTime;
            // target을 향해 이동
            // target 근처로 이동하면 attack State로 변경
        }
        else
        {
            controller.transform.position = controller.attackMoveTarget.position;
            // target = null;

            // idle state로 전환
        }
    }

    public void Exit()
    {
    }
}
