using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour, IControllable
{
    private StateMachine<HeroController> stateMachine;

    private void Awake()
    {
        stateMachine = new StateMachine<HeroController>(this);
        stateMachine.AddState(new IdleState<HeroController>(this));
        stateMachine.AddState(new MoveState<HeroController>(this, new Vector3(-1f, 0f, 0f)));
    }

    private void OnEnable()
    {
        stateMachine.Initialize<MoveState<HeroController>>();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void TransitionToIdleState()
    {
        stateMachine.TransitionTo<IdleState<HeroController>>();
    }
}
