using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

public class ItemSlotController : MonoBehaviour
{
    public RectTransform itemSlotParent;
    public RectTransform itemSelectParent;
    public ItemSlotUI itemSlotPrefab;
    
    private ItemTable itemTable;
    private AssetListTable assetListTable;

    private void Awake()
    {
        itemTable = DataTableManager.Get<ItemTable>(DataTableIds.Item);
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        
    }

    private void Start()
    {
        CreateEmptySlots();
        CreateItemSlots();
    }

    //처음에 2개 빈슬롯을 만듬
    private void CreateEmptySlots()
    {
        for (var i = 0; i < 2; i++)
        {
            var itemSlotInstance = Instantiate(itemSlotPrefab, itemSlotParent);
            itemSlotInstance.Setup(null,null,0);
            itemSlotInstance.onClickItemSlot = HandleItemSlotClick;
        }
    }

    //실제 아이디로 부터 아이템을 생성 
    private void CreateItemSlots()
    {
        var filteredItems = itemTable.table.Values.Where(item => item.Purpose == 1).ToList();

        foreach (var item in filteredItems)
        {
            var itemSlotInstance = Instantiate(itemSlotPrefab, itemSlotParent);

            // AssetListTable에서 AssetNo를 사용하여 assetPath를 가져옴
            if (assetListTable.table.TryGetValue(item.AssetNo, out var assetPath))
            {
                // Variables.LoadTable.ItemId에서 해당 아이템의 개수를 찾음
                var itemCountTuple = Variables.LoadTable.ItemId.FirstOrDefault(tuple => tuple.Item1 == item.Id);
                int itemCount = itemCountTuple?.Item2 ?? 0;

                itemSlotInstance.Setup(item, assetPath, itemCount);
            }
        }

        
    }
    
    private void HandleItemSlotClick(ItemSlotUI clickedSlot)
    {
        if (clickedSlot == null)
        {
            return;
        }

        clickedSlot.ChoicePanel.gameObject.SetActive(!clickedSlot.ChoicePanel.gameObject.activeSelf);
    }
}
