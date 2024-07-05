using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : IState where T : MonoBehaviour, IControllable
{
    private T controller;

    public IdleState(T controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
#if UNITY_EDITOR
        Logger.Log("Idle Enter");
#endif
    }

    public void Update()
    {
    }

    public void Exit()
    {
#if UNITY_EDITOR
        Logger.Log("Idle Exit");
#endif
        
    }
}
