using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MonsterController : CombatEntity<MonsterStatus>, IControllable, ITargetable, IDraggable
{
    private StageManager stageManager;
    public IObjectPool<MonsterController> pool;

    private StateMachine<MonsterController> stateMachine;

    public bool CanPatrol { get; set; }
    public Transform moveTarget { get; set; } // castle 위치
    public Transform attackMoveTarget { get; set; } // 공격 위치
    public PlayerCharacterController attackTarget { get; set; }

    public float findRange = 3f;
    [SerializeField] private bool isDirectedRight = true;
    private float defaultRightScale;

    [HideInInspector] public MonsterSpineAni monsterAni;
    [HideInInspector] public TrackEntry deathTrackEntry;

    public BaseSkill deathSkill;
    public BaseSkill dragDeathSkill;

    private bool isTargetReset=false;
    public bool IsDraggable
    {
        get
        {
            if (IsDead)
                return false;

            var currentState = stateMachine.CurrentState.GetType();
            if (currentState == typeof(FallState) || currentState == typeof(DragState))
                return false;

            switch (Status.Data.DragType)
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
        get
        {
            var currentState = stateMachine.CurrentState.GetType();
            return !IsDead && isTargetReset && currentState != typeof(DragState) && currentState != typeof(FallState);
        }
    }

    public float targetFallY { get; set; }

    public bool TryTransitionState<T>() where T : IState
    {
        if (typeof(T) == typeof(DragState) && !IsDraggable)
            return false;

        return stateMachine.TransitionTo<T>();
    }

    protected override void Awake()
    {
        base.Awake();

        monsterAni = GetComponent<MonsterSpineAni>();

        defaultRightScale = isDirectedRight ? transform.localScale.x : -transform.localScale.x;

        stateMachine = new StateMachine<MonsterController>(this);

        var findBehavior = new FindingTargetInCircle(transform, findRange, 1 << LayerMask.NameToLayer("Player"));
        
        stateMachine.AddState(new IdleState<MonsterController>(this)); // To-Do: 추후 적절하게(Death) 변경
        stateMachine.AddState(new DragState(this));
        stateMachine.AddState(new FallState(this));
        stateMachine.AddState(new MoveState(this));
        stateMachine.AddState(new PatrolState(this, findBehavior));
        stateMachine.AddState(new ChaseState(this));
        stateMachine.AddState(new AttackState(this));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Start()
    {
        var stageManagerGo = GameObject.FindWithTag("StageManager");
        stageManager = stageManagerGo?.GetComponent<StageManager>();

        var castleGo = GameObject.FindWithTag(Defines.Tags.CASTLE_TAG);
        moveTarget = castleGo.transform;

        stateMachine.Initialize<MoveState>();
    }

    public void ResetMonsterData()
    {
        IsDead = false;
        
        stateMachine.TransitionTo<MoveState>();
        Status.Init();
        UpdateHpBar();
        CanPatrol = false;
        isTargetReset=false;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Defines.Tags.PATROL_LINE_TAG))
        {
            CanPatrol = true;
        }

        if (other.CompareTag(Defines.Tags.CASTLE_TAG))
        {
            // if (!IsTargetable)
            //     return;
            
            stageManager?.DamageCastle(10f);
            OnDie();
        }

        if (other.CompareTag("ResetLine"))
        {
            isTargetReset = true;
        }
    }

    public override void Die()
    {
        base.Die();
        if (stageManager)
            stageManager.EarnedGold += Status.Data.DropGold;

        deathSkill?.UseSkill();
            
        deathTrackEntry = monsterAni.SetAnimation(MonsterSpineAni.MonsterState.DEAD, false, 1f);
        stateMachine.TransitionTo<IdleState<MonsterController>>();
        if (deathTrackEntry != null)
        {
            deathTrackEntry.Complete += Die;
        }
    }

    private void Die(TrackEntry entry)
    {
        if (deathTrackEntry != null)
            deathTrackEntry.Complete -= Die;
        OnDie();
    }

    private void OnDie()
    {
        IsDead = true;
        stateMachine.TransitionTo<IdleState<MonsterController>>();
        // attackTarget.TryRemoveMonster(this);
        // stageManager.MonsterCount--;
        // stageManager.EarnedGold += Status.data.DropGold;
        // pool.Release(this);
        
        if (attackTarget) // Null 검사 추가
        {
            attackTarget.TryRemoveMonster(this);
        }

        if (stageManager) // Null 검사 추가
        {
            stageManager.MonsterCount--;
        }
        else
        {
            // Logger.LogError("스테이지 매니저가 할당되지 않았습니다.");
        }

        if (pool != null) // Null 검사 추가
        {
            pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
            // Logger.LogError("풀이 할당되지 않았습니다.");
        }
    }

    public void SetFlip(bool isRight)
    {
        var newScaleX = isRight ? defaultRightScale : defaultRightScale * -1f;
        var transform1 = transform;
        var newScale = new Vector2(newScaleX, transform1.localScale.y);

        transform1.localScale = newScale;
    }

    public bool TryDrag()
    {
        if (!IsDraggable)
            return false;

        return stateMachine.TransitionTo<DragState>();
    }

    public bool TryFall()
    {
        return stateMachine.TransitionTo<FallState>();
    }
}
