using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour, IControllable
{
    private StageManager stageManager;
    public IObjectPool<MonsterController> pool;

    private StateMachine<MonsterController> stateMachine;
    public MonsterStatus Status { get; set; }
    public string testMonsterDragData; // To-Do: 추후 삭제 -> 몬스터의 드래그 타입을 통해 행동 변경

    public Image hpBar;
    private bool isDead = false;

    public bool CanPatrol { get; set; }
    public Transform moveTarget { get; set; } // castle 위치
    public Transform attackMoveTarget { get; set; } // 공격 위치
    public PlayerCharacterController attackTarget { get; set; }

    public float findRange = 3f;
    
    public bool IsDraggable
    {
        get
        {
            if (stateMachine.CurrentState.GetType() == typeof(FallState))
                return false;

            switch (Status.data.DragType)
            {
                case (int)MonsterData.DragTypes.BOSS:
                    return false;
                case (int)MonsterData.DragTypes.NORMAL:
                    return true;
                case (int)MonsterData.DragTypes.SPECIAL:
                    return true; // To-Do: 세이브 데이터로부터 해당 몬스터를 들 수 있는지, 조건문을 더 걸어야 합니다.
                default:
                    return false;
            }
        }
    }

    public bool IsTargetable
    {
        get { return !isDead; }
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
        
        stateMachine.AddState(new IdleState<MonsterController>(this)); // To-Do: 추후 적절하게 변경
        stateMachine.AddState(new DragState(this, dragBehavior));
        stateMachine.AddState(new FallState(this));
        stateMachine.AddState(new MoveState(this));
        stateMachine.AddState(new PatrolState(this, findBehavior));
        stateMachine.AddState(new ChaseState(this));
        stateMachine.AddState(new AttackState(this));

        stateMachine.Initialize<MoveState>();
    }

    private void Start()
    {
        stageManager = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();
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
