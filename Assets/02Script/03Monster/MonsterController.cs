using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, IControllable
{
    private StateMachine<MonsterController> stateMachine;
    public string testMonsterDragData; // 추후 삭제
    
    public bool IsDraggable
    {
        get
        {
            // 세부 조건은 추후 구현
            return true;
        }
    }

    public bool TryTransitionToDragState()
    {
        if (IsDraggable)
        {
            // Drag State로 전환
            stateMachine.TransitionTo<DragState<MonsterController>>();
            return true;
        }

        return false;
    }

    public bool TryTransitionToMoveState()
    {
        return stateMachine.TransitionTo<MoveState<MonsterController>>();
    }

    private void Awake()
    {
        // Idle, Move, Chase, Attack, Drag
        stateMachine = new StateMachine<MonsterController>(this);
        stateMachine.AddState(new IdleState<MonsterController>(this));
        var dragBehavior = TestDragFactory.GenerateDragBehavior(testMonsterDragData, gameObject);
        stateMachine.AddState(new DragState<MonsterController>(this, dragBehavior));
        stateMachine.AddState(new MoveState<MonsterController>(this));
    }

    private void OnEnable()
    {
        stateMachine.Initialize<MoveState<MonsterController>>();
    }

    private void Update()
    {
        stateMachine.Update();

        // 테스트 코드 - 삭제해야함
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stateMachine.TransitionTo<IdleState<MonsterController>>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stateMachine.TransitionTo<MoveState<MonsterController>>();
        }
    }
}
