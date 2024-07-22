using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour, IControllable, IDamageable, ITargetable, IDraggable
{
    private StageManager stageManager;
    public IObjectPool<MonsterController> pool;

    private StateMachine<MonsterController> stateMachine;
    [HideInInspector] public BuffHandler buffHandler;
    public MonsterStatus Status { get; private set; }

    public Image hpBar;
    private bool isDead = false;

    public bool CanPatrol { get; set; }
    public Transform moveTarget { get; set; } // castle 위치
    public Transform attackMoveTarget { get; set; } // 공격 위치
    public PlayerCharacterController attackTarget { get; set; }

    public float findRange = 3f;
    [SerializeField] private bool isDirectedRight = true;
    private float defaultRightScale;
    
    public bool IsDraggable
    {
        get
        {
            if (stateMachine.CurrentState.GetType() == typeof(FallState))
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
            return !isDead && currentState != typeof(DragState) && currentState != typeof(FallState);
        }
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
        defaultRightScale = isDirectedRight ? transform.localScale.x : -transform.localScale.x;

        stateMachine = new StateMachine<MonsterController>(this);

        // var dragBehavior = TestDragFactory.GenerateDragBehavior(testMonsterDragData, gameObject);
        var findBehavior = new FindingTargetInCircle(transform, findRange, 1 << LayerMask.NameToLayer("Player"));
        
        stateMachine.AddState(new IdleState<MonsterController>(this)); // To-Do: 추후 적절하게(Death) 변경
        stateMachine.AddState(new DragState(this));
        stateMachine.AddState(new FallState(this));
        stateMachine.AddState(new MoveState(this));
        stateMachine.AddState(new PatrolState(this, findBehavior));
        stateMachine.AddState(new ChaseState(this));
        stateMachine.AddState(new AttackState(this));

        stateMachine.Initialize<MoveState>();

        buffHandler = new(Status);
        Status = new(buffHandler);
    }

    private void OnEnable()
    {
        Status.OnHpBarUpdate += UpdateHpBar;
        buffHandler.OnDotDamage += TakeDamage;
    }

    private void OnDisable()
    {
        Status.OnHpBarUpdate -= UpdateHpBar;
        buffHandler.OnDotDamage -= TakeDamage;
    }

    private void Start()
    {
        stageManager = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();
    }

    public void ResetMonsterData()
    {
        isDead = false;

        stateMachine.TransitionTo<MoveState>();

        CanPatrol = false;
    }

    private void Update()
    {
        stateMachine.Update();
        Status.buffHandler.TimerUpdate();
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
            
            stageManager.DamageCastle(10f);
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0 || isDead)
            return;
        if (Status == null) // Null 검사 추가
        {
            Logger.LogError("상태가 초기화되지 않았습니다.");
            return;
        }
        Status.CurrentHp -= damage;
        UpdateHpBar();

        if (Status.CurrentHp <= 0f)
        {
            Status.CurrentHp = 0f;
            Die();
        }
    }

    public void TakeBuff(BuffData buffData)
    {
        buffHandler.AddBuff(buffData);
    }

    public void TakeBuff(Buff buff)
    {
        buffHandler.AddBuff(buff);
    }

    private void Die()
    {
        isDead = true;
        // attackTarget.TryRemoveMonster(this);
        // stageManager.MonsterCount--;
        // stageManager.EarnedGold += Status.data.DropGold;
        // pool.Release(this);
        
        if (attackTarget) // Null 검사 추가
        {
            attackTarget.TryRemoveMonster(this);
        }
        else
        {
            Logger.LogError("공격 대상이 할당되지 않았습니다.");
        }

        if (stageManager) // Null 검사 추가
        {
            stageManager.MonsterCount--;
            stageManager.EarnedGold += Status.Data.DropGold;
        }
        else
        {
            Logger.LogError("스테이지 매니저가 할당되지 않았습니다.");
        }

        if (pool != null) // Null 검사 추가
        {
            pool.Release(this);
        }
        else
        {
            Logger.LogError("풀이 할당되지 않았습니다.");
        }
    }

    private void UpdateHpBar()
    {
        if (!hpBar || Status == null)
            return;

        var hpPercent = Status.CurrentHp / Status.Data.Hp;
        hpBar.fillAmount = hpPercent;
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
