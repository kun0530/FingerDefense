using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    public MoveState(T controller, Vector3 dir)
    {
        this.controller = controller;
        direction = dir;
    }

    public MoveState(T controller, GameObject target)
    {
        this.controller = controller;
        this.target = target;
    }

    public void Enter()
    {
    }
    
    public void Update()
    {
        if (target == null)
        {
            controller.transform.position += direction * moveSpeed * Time.deltaTime;

            // 주변 target 감지
        }
        else if (Vector3.Distance(controller.transform.position, target.transform.position) > 0.1)
        {
            
            var dir = controller.transform.position - target.transform.position;
            dir.Normalize();

            controller.transform.position += dir * moveSpeed * Time.deltaTime;
            // target을 향해 이동
            // target 근처로 이동하면 attack State로 변경
        }
        else
        {
            controller.transform.position = target.transform.position;
            target = null;

            // idle state로 전환
        }
    }

    public void Exit()
    {
    }
}
