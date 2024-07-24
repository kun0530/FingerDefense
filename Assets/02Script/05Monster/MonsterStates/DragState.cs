using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragState : IState
{
    private MonsterController controller;
    private MeshRenderer renderer;
    private Collider2D collider;

    private DragAndDrop dragAndDrop;

    public DragState(MonsterController controller)
    {
        this.controller = controller;

        renderer = controller.GetComponentInChildren<MeshRenderer>();
        collider = controller.GetComponent<Collider2D>();
    }

    public void Enter()
    {
        controller.targetFallY = controller.transform.position.y;

        if (controller.attackTarget)
            controller.attackTarget.TryRemoveMonster(controller);

        dragAndDrop = GameObject.FindGameObjectWithTag("InputManager").GetComponent<DragAndDrop>();

        collider.enabled = false;
        renderer.sortingOrder = 1;

        controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.LAYDOWN_AFTER, true, 1f);
    }

    public void Update()
    {
        if (dragAndDrop.IsDragging)
        {
            var pos = Camera.main!.ScreenToWorldPoint(dragAndDrop.GetPointerPosition());
            pos.z = 0f;
            controller.transform.position = pos;
        }
        else
        {
            controller.TryTransitionState<FallState>();
        }
    }

    public void Exit()
    {
        renderer.sortingOrder = 0;
    }
}
