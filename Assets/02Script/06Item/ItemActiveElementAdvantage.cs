using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item/Active Element Advantage", fileName = "Item.asset")]
public class ItemActiveElementAdvantage : ActiveItem
{
    public SpriteRenderer sprite; 
    private List<SpriteRenderer> activeSprites = new();

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

        var characters = GameObject.FindGameObjectsWithTag(Defines.Tags.PLAYER_TAG);
        foreach (var character in characters)
        {
            if (character.TryGetComponent<PlayerCharacterController>(out var controller))
            {
                var elementSprite = Instantiate(sprite, controller.transform.position, Quaternion.identity);
                elementSprite.transform.SetParent(controller.transform);
                activeSprites.Add(elementSprite);
            }
        }
    }

    public override void CancelItem()
    {
        base.CancelItem();

        if (StageMgr)
            StageMgr.isPlayerElementAdvantage = false;

        foreach (var sprite in activeSprites)
        {
            Destroy(sprite);
        }
        activeSprites.Clear();
    }
}
