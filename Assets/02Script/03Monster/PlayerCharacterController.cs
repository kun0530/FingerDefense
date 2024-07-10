using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour, IControllable
{
    private StateMachine<PlayerCharacterController> stateMachine;
    public CharacterStatus Status { get; set; }
    public PlayerCharacterData Data { get; set; }

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

    public bool TryTransitionState<T>() where T : IState
    {
        return stateMachine.TransitionTo<T>();
    }

    private void Awake()
    {
        stateMachine = new StateMachine<PlayerCharacterController>(this);
        stateMachine.AddState(new IdleState<PlayerCharacterController>(this));

        atkTimer = 0f;
    }

    private void OnEnable()
    {
        stateMachine.Initialize<IdleState<PlayerCharacterController>>();

        monsterUp = null;
        monsterDown = null;
    }

    private void Update()
    {
        stateMachine.Update();

        var atkCoolDown = 1f / Status.data.AtkSpeed;
        atkTimer += Time.deltaTime;
        if (atkTimer >= atkCoolDown)
        {
            var findBehavior = new FindingTargetInCircle<MonsterController>(transform, Status.data.AtkRange, 1 << LayerMask.NameToLayer("Monster"));
            var monster = findBehavior.FindTarget() as MonsterController;
            if (monster == null)
                return;
            monster.DamageHp(Status.currentAtkDmg);
            atkTimer = 0f;
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

    public void DamageHp(float damage)
    {
        if (damage < 0)
            return;

        Status.currentHp -= damage;
        UpdateHpBar();

        if (Status.currentHp <= 0f)
        {
            Status.currentHp = 0f;
            Destroy(gameObject);
        }
    }

    private void UpdateHpBar()
    {
        var hpPercent = Status.currentHp / Status.data.Hp;
        hpBar.fillAmount = hpPercent;
    }
}
