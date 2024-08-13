using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Element Advantage", fileName = "Item.asset")]
public class ItemActiveElementAdvantage : ActiveItem
{
    public override void Init()
    {
        base.Init();

        if (StageMgr)
            StageMgr.isPlayerElementAdvantage = false;
    }

    public override void UseItem()
    {
        base.UseItem();

        if (StageMgr)
            StageMgr.isPlayerElementAdvantage = true;
    }

    public override void CancelItem()
    {
        base.CancelItem();

        if (StageMgr)
            StageMgr.isPlayerElementAdvantage = false;
    }
}
