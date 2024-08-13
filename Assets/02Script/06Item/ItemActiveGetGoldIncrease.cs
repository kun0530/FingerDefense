using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Increase Gold", fileName = "Item.asset")]
public class ItemActiveGetGoldIncrease : ActiveItem
{
    public float goldMultiplier;

    public override void UseItem()
    {
        SetGoldMultiplier(goldMultiplier);
        base.UseItem();
    }

    public override void CancelItem()
    {
        SetGoldMultiplier(1f);
        base.CancelItem();
    }

    private void SetGoldMultiplier(float multiplier)
    {
        if (!StageMgr)
            return;

        StageMgr.goldMultiplier = multiplier;
    }
}
