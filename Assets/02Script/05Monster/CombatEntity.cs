using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EntityType
{
    NONE = -1,
    PLAYER_CHARACTER,
    MONSTER
}

public abstract class CombatEntity<T> : MonoBehaviour, IDamageable, IBuffGettable, IEffectGettable where T : BaseStatus, new()
{
    [HideInInspector] public StageManager stageManager;

    public T Status { get; private set; }
    public BuffHandler BuffHandler { get; private set; }

    [Header("공통 특성")]
    public Image hpBar;
    public bool IsDead { get; protected set; }

    protected EntityType entityType = EntityType.NONE;

    [Header("이펙트")]
    public Vector3 effectScale = Vector3.one;
    public Transform effectPosition;
    [HideInInspector] public List<EffectController> effects = new();

    protected virtual void Awake()
    {
        Status = new();
        BuffHandler = new();

        Status.buffHandler = BuffHandler;
    }

    protected virtual void Start()
    {
        var stageManagerGo = GameObject.FindWithTag("StageManager");
        stageManager = stageManagerGo?.GetComponent<StageManager>();
    }

    protected virtual void OnEnable()
    {
        IsDead = false;
        Status.OnHpBarUpdate += UpdateHpBar;
        BuffHandler.OnDotDamage += TakeDamage;
        BuffHandler.OnDotHeal += RecoverHeal;
    }

    protected virtual void OnDisable()
    {
        Status.OnHpBarUpdate -= UpdateHpBar;
        BuffHandler.OnDotDamage -= TakeDamage;
        BuffHandler.OnDotHeal -= RecoverHeal;

        BuffHandler.ResetBuffs();
        foreach (var effect in effects)
        {
            Destroy(effect.gameObject);
        }
        effects.Clear();
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
            effect.transform.SetParent(effectPosition ? effectPosition : transform);
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localScale = effectScale;
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
            effect.transform.SetParent(effectPosition ? effectPosition : transform);
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localScale = effectScale;
            buff.effect = effect;
        }

        return true;
    }

    public virtual bool IsDamageable => !IsDead;

    public bool TakeDamage(float damage, DamageReason reason = DamageReason.NONE, Elements element = Elements.NONE)
    {
        if (!IsDamageable)
            return false;

        if (damage < 0)
            return false;

        if (Status == null)
            return false;

        var damageMultiplier = Utils.ScissorRockPaper(Status.element, element);
        if (stageManager && stageManager.isPlayerElementAdvantage)
        {
            switch (entityType)
            {
                case EntityType.PLAYER_CHARACTER:
                    if (reason == DamageReason.MONSTER_HIT_DAMAGE)
                        damageMultiplier = Mathf.Clamp01(damageMultiplier);
                    break;
                case EntityType.MONSTER:
                    if (reason == DamageReason.PLAYER_HIT_DAMAGE)
                        damageMultiplier = Mathf.Max(damageMultiplier, 1f);
                    break;
            }
        }
        damage *= damageMultiplier;
        Status.CurrentHp -= damage;

        if (Status.CurrentHp <= 0f)
        {
            Die(reason);
        }

        return true;
    }

    public bool RecoverHeal(float heal)
    {
        if (!IsDamageable)
            return false;

        if (heal < 0)
            return false;

        if (Status == null)
            return false;

        Status.CurrentHp += heal;

        if (Status.CurrentHp >= Status.CurrentMaxHp)
        {
            Status.CurrentHp = Status.CurrentMaxHp;
        }

        return true;
    }

    public virtual void Die(DamageReason reason = DamageReason.NONE)
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

    public void AddEffect(EffectController effect)
    {
        if (effects.Contains(effect))
            return;

        effects.Add(effect);
        effect.target = this;
        effect.transform.SetParent(effectPosition);
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localScale = effectScale;
    }

    public void RemoveEffect(EffectController effect)
    {
        if (!effects.Contains(effect))
            return;

        effects.Remove(effect);
    }
}