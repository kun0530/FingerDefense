using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragState : IState
{
    private MonsterController monster;
    private SpriteRenderer renderer;
    private Collider2D collider;
    private IDraggable dragBehavior;

    private DragAndDrop dragAndDrop;

    public DragState(MonsterController monster, IDraggable dragBehavior)
    {
        this.monster = monster;
        this.dragBehavior = dragBehavior;

        renderer = monster.GetComponent<SpriteRenderer>();
        collider = monster.GetComponent<BoxCollider2D>();
    }

    public void Enter()
    {
        monster.targetFallY = monster.transform.position.y;
        dragBehavior?.DragEnter();
        if (monster.attackTarget)
            monster.attackTarget.TryRemoveMonster(monster);

        dragAndDrop = GameObject.FindGameObjectWithTag("InputManager").GetComponent<DragAndDrop>();

        collider.enabled = false;
        renderer.sortingOrder = 1;
    }

    public void Update()
    {
        dragBehavior?.DragUpdate();

        if (dragAndDrop.IsDragging)
        {
            var pos = Camera.main!.ScreenToWorldPoint(dragAndDrop.GetPointerPosition());
            pos.z = 0f;
            monster.transform.position = pos;
        }
        else
        {
            monster.TryTransitionState<FallState>();
        }
    }

    public void Exit()
    {
        dragBehavior?.DragExit();

        renderer.sortingOrder = 0;
    }
}
