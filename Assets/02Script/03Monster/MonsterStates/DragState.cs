using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragState : IState
{
    private MonsterController monster;
    private SpriteRenderer renderer;
    private IDraggable dragBehavior;

    public DragState(MonsterController monster, IDraggable dragBehavior)
    {
        this.monster = monster;
        this.dragBehavior = dragBehavior;

        renderer = monster.GetComponent<SpriteRenderer>();
    }

    public void Enter()
    {
        monster.targetFallY = monster.transform.position.y;
        dragBehavior?.DragEnter();
        monster.attackTarget?.TryRemoveMonster(monster);

        // order layer 앞으로
        renderer.sortingOrder = 1;
    }

    public void Update()
    {
        dragBehavior?.DragUpdate();

        if (Input.GetMouseButton(0))
        {

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
            monster.transform.position = pos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            monster.TryTransitionState<FallState>();
        }
    }

    public void Exit()
    {
        dragBehavior?.DragExit();

        // order layer 원래대로
        renderer.sortingOrder = 0;
    }
}
