using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Restore Castle HP", fileName = "Item.asset")]
public class ItemRestoreCastle : BaseItem
{
    public bool isPercentage;
    public float restoreCastleValue;

    public override bool UseItem()
    {
        StageManager?.RestoreCastle(restoreCastleValue, isPercentage);
        return true;
    }
}
