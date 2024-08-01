using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CombatEntity<T> : MonoBehaviour, IDamageable, IBuffGettable where T : BaseStatus, new()
{
    public T Status { get; protected set; }
    protected BuffHandler buffHandler;

    public Image hpBar;
    public bool IsDead { get; protected set; }

    protected virtual void Awake()
    {
        Status = new();
        buffHandler = new();

        Status.buffHandler = buffHandler;
    }

    protected virtual void OnEnable()
    {
        IsDead = false;
        buffHandler.ResetBuffs();
        Status.OnHpBarUpdate += UpdateHpBar;
        buffHandler.OnDotDamage += TakeDamage;
    }

    protected virtual void OnDisable()
    {
        Status.OnHpBarUpdate -= UpdateHpBar;
        buffHandler.OnDotDamage -= TakeDamage;
    }

    protected virtual void Update()
    {
        buffHandler.TimerUpdate();
    }

    public virtual bool IsBuffGettable => !IsDead;

    public bool TakeBuff(BuffData buffData)
    {
        if (!IsBuffGettable)
            return false;

        return buffHandler.AddBuff(buffData);
        // EffectFactoryTest.CreateEffect(buffData.EffectNo.ToString(), gameObject);
    }

    public bool TakeBuff(Buff buff)
    {
        if (!IsBuffGettable)
            return false;

        return buffHandler.AddBuff(buff);
        // EffectFactoryTest.CreateEffect(buff.buffData.EffectNo.ToString(), gameObject);
    }

    public virtual bool IsDamageable => !IsDead;

    public bool TakeDamage(float damage)
    {
        if (!IsDamageable)
            return false;

        if (damage < 0)
            return false;

        if (Status == null)
            return false;

        Status.CurrentHp -= damage;
        UpdateHpBar();

        if (Status.CurrentHp <= 0f)
        {
            Die();
        }

        return true;
    }

    public virtual void Die(bool isDamageDeath = true)
    {
        IsDead = true;
        Status.CurrentHp = 0f;
    }

    protected void UpdateHpBar()
    {
        if (!hpBar)
        {
            Logger.LogError($"HP Bar가 할당되지 않았습니다: {gameObject.name}");
            return;
        }

        var hpPercent = Status.CurrentHp / Status.CurrentMaxHp;
        hpBar.fillAmount = hpPercent;
    }
}