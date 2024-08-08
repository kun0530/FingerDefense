using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Passive Get Castle Shield", fileName = "Item.asset")]
public class ItemPassiveGetCastleShield : BaseItem
{
    public override bool IsPassive { get => true; }

    public bool isPercentage;
    public float castleShield;

    public override bool UseItem()
    {
        StageManager?.GetShield(castleShield, isPercentage);
        return true;
    }
}
