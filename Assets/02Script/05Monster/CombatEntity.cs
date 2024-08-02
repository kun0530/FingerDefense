using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CombatEntity<T> : MonoBehaviour, IDamageable, IBuffGettable where T : BaseStatus, new()
{
    public T Status { get; private set; }
    public BuffHandler BuffHandler { get; private set; }

    public Image hpBar;
    public bool IsDead { get; protected set; }

    protected virtual void Awake()
    {
        Status = new();
        BuffHandler = new();

        Status.buffHandler = BuffHandler;
    }

    protected virtual void OnEnable()
    {
        IsDead = false;
        BuffHandler.ResetBuffs();
        Status.OnHpBarUpdate += UpdateHpBar;
        BuffHandler.OnDotDamage += TakeDamage;
    }

    protected virtual void OnDisable()
    {
        Status.OnHpBarUpdate -= UpdateHpBar;
        BuffHandler.OnDotDamage -= TakeDamage;
    }

    protected virtual void Update()
    {
        BuffHandler.TimerUpdate();
    }

    public virtual bool IsBuffGettable => !IsDead;

    public bool TakeBuff(BuffData buffData)
    {
        if (!IsBuffGettable)
            return false;

        if (!BuffHandler.TryAddBuff(buffData, out var buff))
            return false;

        var effect = EffectFactory.CreateEffect(buffData.EffectNo);
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.transform.SetParent(transform);
            buff.effect = effect;
        }

        return true;
    }

    public bool TryTakeBuff(BuffData buffData, out Buff buff, bool isTimerStop = false)
    {
        buff = null;

        if (!IsBuffGettable)
            return false;

        if (!BuffHandler.TryAddBuff(buffData, out buff, isTimerStop))
            return false;

        var effect = EffectFactory.CreateEffect(buffData.EffectNo);
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.transform.SetParent(transform);
            buff.effect = effect;
        }

        return true;
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