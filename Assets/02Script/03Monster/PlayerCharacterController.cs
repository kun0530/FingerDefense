using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour, IControllable
{
    private StateMachine<PlayerCharacterController> stateMachine;
    public PlayerCharacterData Data { get; set; }

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
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
