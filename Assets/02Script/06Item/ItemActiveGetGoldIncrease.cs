using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Increase Gold", fileName = "Item.asset")]
public class ItemActiveGetGoldIncrease : BaseItem
{
    public float goldMultiplier;
    public float lastingTime;
    private float timer;

    public override bool UseItem()
    {
        return SetGoldMultiplier(goldMultiplier);
    }

    public override bool CancelItem()
    {
        return SetGoldMultiplier(1f);
    }

    public override void UpdateItem()
    {
        timer += Time.deltaTime;
        if (timer >= lastingTime)
        {
            CancelItem();
        }
    }

    private bool SetGoldMultiplier(float multiplier)
    {
        if (!StageManager)
            return false;

        StageManager.goldMultiplier = multiplier;

        timer = 0f;
        return true;
    }
}
