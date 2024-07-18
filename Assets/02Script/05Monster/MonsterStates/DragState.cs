using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragState : IState
{
    private MonsterController monster;
    private SpriteRenderer renderer;
    private Collider2D collider;

    private DragAndDrop dragAndDrop;

    public DragState(MonsterController monster)
    {
        this.monster = monster;

        renderer = monster.GetComponent<SpriteRenderer>();
        collider = monster.GetComponent<BoxCollider2D>();
    }

    public void Enter()
    {
        monster.targetFallY = monster.transform.position.y;

        if (monster.attackTarget)
            monster.attackTarget.TryRemoveMonster(monster);

        dragAndDrop = GameObject.FindGameObjectWithTag("InputManager").GetComponent<DragAndDrop>();

        collider.enabled = false;
        renderer.sortingOrder = 1;
    }

    public void Update()
    {
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
        renderer.sortingOrder = 0;
    }
}
