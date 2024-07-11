using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour, IControllable
{
    public IObjectPool<MonsterController> pool;

    private StateMachine<MonsterController> stateMachine;
    public MonsterStatus Status { get; set; }
    public string testMonsterDragData; // 몬스터의 드래그 타입을 통해 행동 변경 -> 추후 삭제

    public Image hpBar;
    private bool isDead = false;

    public bool CanPatrol { get; set; }
    public Transform moveTarget { get; set; }
    public Transform attackMoveTarget { get; set; }
    public PlayerCharacterController attackTarget { get; set; }

    public float findRange = 3f;
    
    public bool IsDraggable
    {
        get
        {
            if (Status.data.DragType <= (int)MonsterData.DragTypes.None
            || Status.data.DragType >= (int)MonsterData.DragTypes.Count)
                return false;

            if (stateMachine.CurrentState.GetType() == typeof(FallState))
                return false;

            switch ((MonsterData.DragTypes)Status.data.DragType)
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

    public bool IsTargetable
    {
        get { return true; }
    }
    public float targetFallY { get; set; }

    public bool TryTransitionState<T>() where T : IState
    {
        if (typeof(T) == typeof(DragState) && !IsDraggable)
            return false;

        return stateMachine.TransitionTo<T>();
    }

    private void Awake()
    {
        stateMachine = new StateMachine<MonsterController>(this);

        var dragBehavior = TestDragFactory.GenerateDragBehavior(testMonsterDragData, gameObject);
        var findBehavior = new FindingTargetInCircle<PlayerCharacterController>(transform, findRange, 1 << LayerMask.NameToLayer("Player"));
        
        stateMachine.AddState(new IdleState<MonsterController>(this)); // To-Do: 삭제
        stateMachine.AddState(new DragState(this, dragBehavior));
        stateMachine.AddState(new FallState(this));
        stateMachine.AddState(new MoveState(this));
        stateMachine.AddState(new PatrolState(this, findBehavior));
        stateMachine.AddState(new ChaseState(this));
        stateMachine.AddState(new AttackState(this));

        stateMachine.Initialize<MoveState>();
    }

    public void ResetMonsterData()
    {
        isDead = false;

        stateMachine.TransitionTo<MoveState>();
        Status.Init();
        UpdateHpBar();

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

        if (other.CompareTag("Castle"))
        {
            var stageManager = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();
            stageManager.DamageCastle(10f);
            stageManager.MonsterCount--;
            pool.Release(this);
        }
    }

    public void DamageHp(float damage)
    {
        if (damage < 0 || isDead)
            return;

        Status.currentHp -= damage;
        UpdateHpBar();

        if (Status.currentHp <= 0f)
        {
            Status.currentHp = 0f;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        attackTarget?.TryRemoveMonster(this);

        var stageManager = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();
        stageManager.MonsterCount--;
        
        pool.Release(this);
    }

    private void UpdateHpBar()
    {
        if (hpBar == null || Status == null)
            return;

        var hpPercent = Status.currentHp / Status.data.Hp;
        hpBar.fillAmount = hpPercent;
    }
}
