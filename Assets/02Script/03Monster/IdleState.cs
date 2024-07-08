using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : IState where T : PlayerCharacterController
{
    private T controller;

    private float patrolTimer = 0f;
    private float patrolInterval = 0.25f;

    public IdleState(T controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        patrolTimer = 0f;
    }

    public void Update()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolInterval)
        {
            patrolTimer = 0f;

            var colliders = Physics2D.OverlapCircleAll(controller.transform.position, controller.Data.AtkRange);
            MonsterController nearCollider = null;
            float nearDistance = float.MaxValue;
            foreach (var col in colliders)
            {
                if (col.TryGetComponent<MonsterController>(out var monster))
                {
                    float distance = Vector2.Distance(monster.transform.position, controller.transform.position);
                    if (distance < nearDistance)
                    {
                        nearCollider = monster;
                        nearDistance = distance;
                    }
                }
            }

            if (nearCollider != null)
            {
                // attack state로 변경
            }
        }
    }

    public void Exit()
    {
        Logger.Log("Idle Exit");
    }
}
