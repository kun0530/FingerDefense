using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour, IControllable, IDamageable, ITargetable
{
    public PlayerCharacterSpawner spawner { get; set; }

    public CharacterStatus Status { get; set; }
    public bool IsDead { get; set; } = true;

    private MonsterController atkTarget;
    private float atkTimer;
    public Image hpBar;

    public Transform[] mosnterPosition;

    public MonsterController monsterUp { get; set; }
    public MonsterController monsterDown { get; set; }

    public int MonsterCount
    {
        get
        {
            int count = 0;
            if (monsterUp != null)
                count++;
            if (monsterDown != null)
                count++;
            return count;
        }
    }
    public bool IsTargetable
    {
        get { return MonsterCount != 2; }
    }

    private void Awake()
    {
        atkTimer = 0f;
    }

    private void OnEnable()
    {
        atkTarget = null;
        monsterUp = null;
        monsterDown = null;

        Status?.Init();
        UpdateHpBar();
    }

    public void ResetPlayerData()
    {
        IsDead = false;
    }

    private void FixedUpdate()
    {
        var findBehavior = new FindingTargetInCircle(transform, Status.data.AtkRange, 1 << LayerMask.NameToLayer("Monster"));
        var nearCollider = findBehavior.FindTarget();
        if (nearCollider == null)
        {
            atkTarget = null;
            return;
        }
        
        if (nearCollider.TryGetComponent<MonsterController>(out var target))
        {
            atkTarget = target;
        }
        else
        {
            atkTarget = null;
        }
    }

    private void Update()
    {
        var atkCoolDown = 1f / Status.data.AtkSpeed;
        atkTimer += Time.deltaTime;
        if (atkTimer >= atkCoolDown)
        {
            atkTarget?.TakeDamage(Status.currentAtkDmg);
            atkTimer = 0f;

            // 스킬이 준비되면, 일반 스킬은 일시 중지
            // 스킬 캐스팅
        }
    }

    public bool TryAddMonster(MonsterController monster)
    {
        if (monster == monsterUp || monster == monsterDown)
            return false;

        if (MonsterCount == 2)
            return false;

        if (monsterUp == null)
            monsterUp = monster;
        else
            monsterDown = monster;

        monster.attackTarget = this;
        UpdateMonsterPosition();
        return true;
    }

    public bool TryRemoveMonster(MonsterController monster)
    {
        if (monster == null)
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

    public void UpdateMonsterPosition()
    {
        switch (MonsterCount)
        {
            case 0:
                return;
            case 1:
                {
                    if (monsterUp == null)
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

        Status.currentHp -= damage;
        UpdateHpBar();

        if (Status.currentHp <= 0f)
        {
            TryRemoveMonster(monsterUp);
            TryRemoveMonster(monsterDown);
            Status.currentHp = 0f;
            IsDead = true;
            spawner.RemoveActiveCharacter(this);
            gameObject.SetActive(false);
        }
    }

    private void UpdateHpBar()
    {
        if (hpBar == null || Status == null)
            return;

        var hpPercent = Status.currentHp / Status.data.Hp;
        hpBar.fillAmount = hpPercent;
    }
}
