using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Restore Castle HP", fileName = "Item.asset")]
public class ItemActiveRestoreCastle : ActiveItem
{
    public bool isPercentage;
    public float restoreCastleValue;

    public override void UseItem()
    {
        StageMgr?.RestoreCastle(restoreCastleValue, isPercentage);
        if (effectPrefab)
        {
            var effect = Instantiate(effectPrefab, effectPos, Quaternion.identity);
            effect.LifeTime = duration;
        }
        base.UseItem();
    }
}
