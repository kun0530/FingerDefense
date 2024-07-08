using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour, IControllable
{
    private StateMachine<PlayerCharacterController> stateMachine;
    public PlayerCharacterData Data { get; set; }

    public Transform[] mosnterPosition;
    public int monsterCount { get; set; } = 0;
    public MonsterController[] monsters { get; set; } = new MonsterController[2];

    public bool TryTransitionState<T>() where T : IState
    {
        return stateMachine.TransitionTo<T>();
    }

    private void Awake()
    {
        stateMachine = new StateMachine<PlayerCharacterController>(this);
        stateMachine.AddState(new IdleState<PlayerCharacterController>(this));
    }

    private void OnEnable()
    {
        stateMachine.Initialize<IdleState<PlayerCharacterController>>();

        monsterCount = 0;
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = null;
        }
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public bool TryAddMonster(MonsterController monster)
    {
        foreach (var mon in monsters) // 이미 존재하는 경우
        {
            if (mon == monster)
                return false;
        }

        switch (monsterCount)
        {
            case 0:
                {
                    monsters[0] = monster;
                    monster.attackMoveTarget = mosnterPosition[0];
                }
                break;
            case 1:
                {
                    monsters[1] = monster;
                    monsters[0].attackMoveTarget = mosnterPosition[1];
                    monster.attackMoveTarget = mosnterPosition[2];
                }
                break;
            case 2:
                return false;
        }

        monster.attackTarget = this;
        monsterCount++;
        return true;
    }
}
