using System.Collections.Generic;
using UnityEngine;

public class DeckSlotCreate : MonoBehaviour
{
    public RectTransform characterSlotParent;
    public RectTransform itemSlotParent;
    public RectTransform filterringSlotParent;
    
    private PlayerCharacterTable playerCharacterTable;
    
    public CharacterSlotUI characterSlotPrefab;
    //public ItemSlotUI itemSlotPrefab;
    private void Start()
    {
        playerCharacterTable ??= DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        
        CreateCharacterSlots();
        CreateItemSlots();
        CreateFilteringSlots();
    }
    
    private void CreateCharacterSlots()
    {
        
    }
    
    private void CreateItemSlots()
    {
        
    }
    
    private void CreateFilteringSlots()
    {
        
    }
}
