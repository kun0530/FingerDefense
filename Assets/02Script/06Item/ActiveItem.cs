using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveItem : BaseItem
{
    public override bool IsPassive { get => false; }

    public float duration;
    protected bool isItemUsed = false;
    protected float durationTimer = 0f;

    public float coolDown;
    protected bool isCoolDown = true;
    protected float coolDownTimer = 0f;

    public override void UseItem()
    {
        count--;

        isItemUsed = true;
        durationTimer = 0f;

        isCoolDown = false;
        coolDownTimer = 0f;

        button.interactable = false;
    }

    public override void CancelItem()
    {
        isItemUsed = false;
    }

    public override void UpdateItem()
    {
        UpdateDurationTimer();
        UpdateCoolDownTimer();
    }

    private void UpdateDurationTimer()
    {
        if (!isItemUsed)
            return;

        durationTimer += Time.deltaTime;
        if (durationTimer >= duration)
        {
            CancelItem();
        }
    }

    private void UpdateCoolDownTimer()
    {
        if (count == 0 || isCoolDown)
            return;

        coolDownTimer += Time.deltaTime;
        if (coolDownTimer >= coolDown)
        {
            button.interactable = true;
            isCoolDown = true;
        }
    }
}
