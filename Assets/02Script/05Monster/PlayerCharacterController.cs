using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour, IControllable, IDamageable, ITargetable
{
    public PlayerCharacterSpawner spawner;

    public CharacterStatus Status { get; private set; }
    public BuffHandler buffHandler;
    
    public bool IsDead { get; set; } = true;

    // private MonsterController atkTarget;
    // private float atkTimer = 0f;
    public Image hpBar;

    // public BaseSkill skill;
    // public SkillData skillData;
    // private float skillTimer;

    public Transform[] mosnterPosition;

    public MonsterController monsterUp { get; set; }
    public MonsterController monsterDown { get; set; }

    private CharacterSpineAni anim;
    private TrackEntry deathTrackEntry;

    private int MonsterCount
    {
        get
        {
            int count = 0;
            if (monsterUp)
                count++;
            if (monsterDown)
                count++;
            return count;
        }
    }
    public bool IsTargetable => MonsterCount != 2;

    private void Awake()
    {
        Status = new(buffHandler);
        buffHandler = new(Status);

        anim = GetComponent<CharacterSpineAni>();
    }

    private void OnEnable()
    {
        // atkTarget = null;
        monsterUp = null;
        monsterDown = null;

        IsDead = false;

        Status.OnHpBarUpdate += UpdateHpBar;
        buffHandler.OnDotDamage += TakeDamage;
    }

    private void OnDisable()
    {
        Status.OnHpBarUpdate -= UpdateHpBar;
        buffHandler.OnDotDamage -= TakeDamage;
    }

    private void FixedUpdate()
    {
        if (Status.Data == null)
            return;
            
        // var findBehavior = new FindingTargetInCircle(transform, Status.Data.AtkRange, 1 << LayerMask.NameToLayer("Monster"));
        // var nearCollider = findBehavior.FindTarget();
        // if (!nearCollider)
        // {
        //     atkTarget = null;
        //     return;
        // }
        
        // // 코드 ?: 연산자로 변경 : 방민호
        // atkTarget = nearCollider.TryGetComponent<MonsterController>(out var target) ? target : null;
    }

    private void Update()
    {
        // var atkCoolDown = 1f / Status.Data.AtkSpeed;
        // atkTimer += Time.deltaTime;
        // if (atkTarget && atkTimer >= atkCoolDown && !IsDead)
        // {
        //     anim.SetAnimation(CharacterSpineAni.CharacterState.ATTACK, false, 0.1f);
        //     atkTarget?.TakeDamage(Status.currentAtkDmg);
        //     atkTimer = 0f;

        //     // 스킬이 준비되면, 일반 스킬은 일시 중지
        //     // 스킬 캐스팅
        // }
        // // 테스트용 코드 : 공격중이 아니면 Idle 상태 유지 (방민호)
        // else
        // {
        //     anim.SetAnimation(CharacterSpineAni.CharacterState.IDLE, true, 0.1f);
        // }
        
        // 에러로 인해 비활성화 : 방민호
        // if (skill != null && skillData != null)
        // {
        //     skillTimer += Time.deltaTime;
        //     if (skillTimer >= skillData.CoolTime)
        //     {
        //         skill.UseSkill();
        //         skillTimer = 0f;
        //     }
        // }
        
        if (IsDead)
        {
            gameObject.SetActive(false);
        }
        buffHandler.TimerUpdate();
    }

    public bool TryAddMonster(MonsterController monster)
    {
        if (monster == monsterUp || monster == monsterDown)
            return false;

        if (MonsterCount == 2)
            return false;

        if (!monsterUp)
            monsterUp = monster;
        else
            monsterDown = monster;

        monster.attackTarget = this;
        UpdateMonsterPosition();
        return true;
    }

    public bool TryRemoveMonster(MonsterController monster)
    {
        if (!monster)
            return false;

        if (monsterUp != monster && monsterDown != monster)
            return false;

        if (monsterUp == monster)
            monsterUp = null;
        else
            monsterDown = null;

        monster.attackMoveTarget = null;
        monster.attackTarget = null;
        UpdateMonsterPosition();
        return true;
    }

    private void UpdateMonsterPosition()
    {
        switch (MonsterCount)
        {
            case 0:
                return;
            case 1:
                {
                    if (!monsterUp)
                    {
                        monsterUp = monsterDown;
                        monsterDown = null;
                    }
                    monsterUp.attackMoveTarget = mosnterPosition[0];
                }
                break;
            case 2:
                {
                    monsterUp.attackMoveTarget = mosnterPosition[1];
                    monsterDown.attackMoveTarget = mosnterPosition[2];
                }
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
            return;

        Status.CurrentHp -= damage;
        UpdateHpBar();

        if (Status.CurrentHp <= 0f)
        {
            TryRemoveMonster(monsterUp);
            TryRemoveMonster(monsterDown);
            Status.CurrentHp = 0f;
            IsDead = true;
            
            // PASSOUT 상태로 변경 : 방민호
            PlayDeathAnimation();
            
            //현재 비활성화 하는 부분 주석처리하고 , PASSOUT 상태가 끝나면 이벤트를 통해 IsDead를 True로 변경해서 반응하도록 수정'
            //위에 Update에서 확인가능 : 방민호
            // gameObject.SetActive(false);
        }
    }

    public void PlayDeathAnimation()
    {
        deathTrackEntry = anim.SetAnimation(CharacterSpineAni.CharacterState.PASSOUT, false, 1f);
        if (deathTrackEntry != null)
            deathTrackEntry.Complete += Die;
    }

    private void Die(TrackEntry trackEntry)
    {
        spawner?.RemoveActiveCharacter(this);
        if (deathTrackEntry != null)
            deathTrackEntry.Complete -= Die;
    }

    public void TakeBuff(BuffData buffData)
    {
        buffHandler.AddBuff(buffData);
    }

    public void TakeBuff(Buff buff)
    {
        buffHandler.AddBuff(buff);
    }

    private void UpdateHpBar()
    {
        if (!hpBar)
        {
            Logger.LogError($"HP Bar가 할당되었는지 확인해주세요: {gameObject.name}");
            return;
        }

        var hpPercent = Status.CurrentHp / Status.Data.Hp;
        hpBar.fillAmount = hpPercent;
    }
}
