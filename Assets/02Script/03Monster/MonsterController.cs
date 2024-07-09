using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterController : MonoBehaviour, IControllable
{
    public IObjectPool<MonsterController> pool;

    private StateMachine<MonsterController> stateMachine;
    public MonsterData Data { get; set; }
    public string testMonsterDragData; // 추후 삭제

    public bool CanPatrol { get; set; }
    public Transform moveTarget { get; set; }
    public Transform attackMoveTarget { get; set; }
    public PlayerCharacterController attackTarget { get; set; }
    
    public bool IsDraggable
    {
        get
        {
            if (Data == null || Data.DragType <= (int)MonsterData.DragTypes.None || Data.DragType >= (int)MonsterData.DragTypes.Count)
                return false;

            switch ((MonsterData.DragTypes)Data.DragType)
            {
                case MonsterData.DragTypes.BOSS:
                    return false;
                case MonsterData.DragTypes.NORMAL:
                    return true;
                case MonsterData.DragTypes.SPECIAL:
                    return true; // To-Do: 세이브 데이터로부터 해당 몬스터를 들 수 있는지, 조건문을 더 걸어야 합니다.
                default:
                    return false;
            }
        }
    }

    public bool TryTransitionState<T>() where T : IState
    {
        if (typeof(T) == typeof(DragState<MonsterController>) && !IsDraggable)
            return false;

        return stateMachine.TransitionTo<T>();
    }

    private void Awake()
    {
        // Idle, Move, Chase, Attack, Drag, Fall
        stateMachine = new StateMachine<MonsterController>(this);
        var dragBehavior = TestDragFactory.GenerateDragBehavior(testMonsterDragData, gameObject);
        stateMachine.AddState(new IdleState<MonsterController>(this));
        stateMachine.AddState(new DragState<MonsterController>(this, dragBehavior));
        stateMachine.AddState(new MoveState(this));
        stateMachine.AddState(new PatrolState(this));
    }

    private void OnEnable()
    {
        stateMachine.Initialize<MoveState>();
        CanPatrol = false;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PatrolStartLine"))
        {
            CanPatrol = true;
        }
    }
}
