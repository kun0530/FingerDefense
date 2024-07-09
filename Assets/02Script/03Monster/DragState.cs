using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragState<T> : IState where T : MonsterController
{
    private T controller;
    private IDraggable dragBehavior;

    public DragState(T controller, IDraggable dragBehavior)
    {
        this.controller = controller;
        this.dragBehavior = dragBehavior;
    }

    public void Enter()
    {
        dragBehavior?.DragEnter();

        controller.attackTarget?.TryRemoveMonster(controller);
    }

    public void Update()
    {
        dragBehavior?.DragUpdate();
    }

    public void Exit()
    {
        dragBehavior?.DragExit();
    }
}
