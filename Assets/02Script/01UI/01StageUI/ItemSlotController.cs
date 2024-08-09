using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSlotController : MonoBehaviour
{
    public RectTransform itemSlotParent;  // 빈 슬롯이 위치할 부모 객체
    public RectTransform itemSelectParent;  // 아이템 슬롯이 위치할 부모 객체
    public ItemSlotUI itemSlotPrefab;

    private ItemTable itemTable;
    private AssetListTable assetListTable;

    private List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();  // 아이템 슬롯 리스트
    private List<ItemSlotUI> emptySlots = new List<ItemSlotUI>();  // 빈 슬롯 리스트
    private HashSet<int> addedItems = new HashSet<int>();  // 추가된 아이템 ID를 관리
    private List<ItemSlotUI> activeChoicePanelSlots = new List<ItemSlotUI>();  // ChoicePanel이 활성화된 슬롯 리스트

    private void Awake()
    {
        itemTable = DataTableManager.Get<ItemTable>(DataTableIds.Item);
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
    }

    private void Start()
    {
        RefreshItemSlots();
    }

    private void RefreshItemSlots()
    {
        foreach (var slot in itemSlots)
        {
            Destroy(slot.gameObject);
        }
        itemSlots.Clear();

        foreach (var slot in emptySlots)
        {
            Destroy(slot.gameObject);
        }
        emptySlots.Clear();
        addedItems.Clear();
        activeChoicePanelSlots.Clear();

        // 새로운 슬롯 생성
        CreateEmptySlots();
        CreateItemSlots();
    }

    private void CreateEmptySlots()
    {
        for (var i = 0; i < 2; i++)
        {
            AddEmptySlot(i);
        }
    }

    private void CreateItemSlots()
    {
        var filteredItems = itemTable.table.Values.Where(item => item.Purpose == 1).ToList();

        foreach (var item in filteredItems)
        {
            var itemSlot = Instantiate(itemSlotPrefab, itemSelectParent);

            if (assetListTable.table.TryGetValue(item.IconNo, out var assetPath))
            {
                var itemCountTuple = Variables.LoadTable.ItemId.FirstOrDefault(tuple => tuple.Item1 == item.Id);
                int itemCount = itemCountTuple?.Item2 ?? 0;

                itemSlot.Setup(item, assetPath, itemCount);
                itemSlot.onClickItemSlot = clickedSlot => HandleItemSlotClick(clickedSlot, -1); 
                itemSlots.Add(itemSlot);
            }
        }
    }

    private void AddEmptySlot(int slotIndex)
    {
        var slot = Instantiate(itemSlotPrefab, itemSlotParent);
        slot.Setup(null, null, 0);
        slot.onClickItemSlot = clickedSlot => HandleItemSlotClick(clickedSlot, slotIndex);
        emptySlots.Add(slot);
    }

    private void HandleItemSlotClick(ItemSlotUI clickedSlot, int slotIndex)
    {
        if (clickedSlot == null) return;

        if (clickedSlot.transform.parent == itemSelectParent)
        {
            if (addedItems.Contains(clickedSlot.ItemId))
            {
                Debug.Log("이미 추가된 아이템입니다.");
                return;
            }

            foreach (var slot in emptySlots)
            {
                if (slot.ItemId == 0)
                {
                    slot.SetItemSlot(clickedSlot.ItemId, clickedSlot.ItemSprite);
                    slot.ChoicePanel.transform.gameObject.SetActive(false);
                    clickedSlot.ToggleInteractable(false);
                    addedItems.Add(clickedSlot.ItemId);
                    activeChoicePanelSlots.Add(clickedSlot);
                    UpdateChoicePanels();

                    // 아이템을 해당 슬롯에 추가
                    AddItemToSlot(slotIndex, clickedSlot.ItemId, clickedSlot.GetOriginalLimit());
                    break;
                }
            }
        }
        else if (clickedSlot.transform.parent == itemSlotParent)
        {
            var originalSlot = activeChoicePanelSlots.FirstOrDefault(slot => slot.ItemId == clickedSlot.ItemId);
            if (originalSlot != null)
            {
                originalSlot.ToggleInteractable(true);
                activeChoicePanelSlots.Remove(originalSlot);
                addedItems.Remove(originalSlot.ItemId);
                UpdateChoicePanels();

                // 아이템을 해당 슬롯에서 제거
                RemoveItemFromSlot(slotIndex, originalSlot.ItemId);
            }

            clickedSlot.ClearSlot();
            emptySlots.Remove(clickedSlot);
            Destroy(clickedSlot.gameObject);
            AddEmptySlot(slotIndex);
        }
    }

    private void AddItemToSlot(int slotIndex, int itemId, int limit)
    {
        if (slotIndex == 0)
        {
            
            Variables.LoadTable.ItemId.Add(new Tuple<int, int>(itemId, limit));
            Logger.Log($"Added item {itemId} to ItemId, limit {limit}");
        }
        else if (slotIndex == 1)
        {
            Variables.LoadTable.ItemId2.Add(new Tuple<int, int>(itemId, limit));
            Logger.Log($"Added item {itemId} to ItemId2, limit {limit}");
        }
    }
    private void RemoveItemFromSlot(int slotIndex, int itemId)
    {
        if (slotIndex == 0)
        {
            var itemToRemove = Variables.LoadTable.ItemId.FirstOrDefault(tuple => tuple.Item1 == itemId);
            if (itemToRemove != null)
            {
                Variables.LoadTable.ItemId.Remove(itemToRemove);
                Logger.Log($"Removed item {itemId} from ItemId");
            }
        }
        else if (slotIndex == 1)
        {
            var itemToRemove = Variables.LoadTable.ItemId2.FirstOrDefault(tuple => tuple.Item1 == itemId);
            if (itemToRemove != null)
            {
                Variables.LoadTable.ItemId2.Remove(itemToRemove);
                Logger.Log($"Removed item {itemId} from ItemId2");
            }
        }
    }
    
    private void UpdateChoicePanels()
    {
        // 모든 슬롯의 ChoicePanel을 비활성화
        foreach (var slot in itemSlots)
        {
            slot.ChoicePanel.transform.gameObject.SetActive(false);
        }

        // 활성화된 ChoicePanel 슬롯만 다시 활성화
        foreach (var slot in activeChoicePanelSlots)
        {
            slot.ChoicePanel.transform.gameObject.SetActive(true);
        }
    }
}

    
