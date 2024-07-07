using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour, IControllable
{
    private StateMachine<HeroController> stateMachine;

    public bool TryTransitionState<T>() where T : IState
    {
        return stateMachine.TransitionTo<T>();
    }

    private void Awake()
    {
        stateMachine = new StateMachine<HeroController>(this);
        stateMachine.AddState(new IdleState<HeroController>(this));
    }

    private void OnEnable()
    {
        stateMachine.Initialize<IdleState<HeroController>>();
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
