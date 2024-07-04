using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState<T> : IState where T : MonoBehaviour, IControllable
{
    private T controller;
    private float moveSpeed = 1f;
    private Vector3 direction = new Vector3(1f, 0f, 0f);

    private GameObject target;

    public MoveState(T controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
    }
    
    public void Update()
    {
        if (target == null)
        {
            controller.transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            // target을 향해 이동
            // target 근처로 이동하면 attack State로 변경
        }

        // 주변 target 감지
    }

    public void Exit()
    {
    }
}
