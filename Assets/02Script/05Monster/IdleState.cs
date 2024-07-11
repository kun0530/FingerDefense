using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : IState where T : IControllable
{
    private T controller;

    public IdleState(T controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
