using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Active Monster Debuff", fileName = "Item.asset")]
public class ItemActiveDebuffMonster : ActiveItem
{
    [SerializeField] public List<ItemDebuffMonster> debuffs;

    public override void UseItem()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        if (monsters == null)
            return;

        foreach (var monster in monsters)
        {
            if (!monster.TryGetComponent<MonsterController>(out var controller))
                continue;
            foreach (var debuff in debuffs)
            {
                debuff.GiveBuff(controller);
            }
        }
        
        base.UseItem();
    }
}
