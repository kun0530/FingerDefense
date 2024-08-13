using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSlotController : MonoBehaviour
{
    public RectTransform itemSlotParent; // 빈 슬롯이 위치할 부모 객체
    public RectTransform itemSelectParent; // 아이템 슬롯이 위치할 부모 객체
    public ItemSlotUI itemSlotPrefab;

    private ItemTable itemTable;
    private AssetListTable assetListTable;

    private List<ItemSlotUI> itemSlots = new List<ItemSlotUI>(); // 아이템 슬롯 리스트
    private List<ItemSlotUI> emptySlots = new List<ItemSlotUI>(); // 빈 슬롯 리스트
    private HashSet<int> addedItems = new HashSet<int>(); // 추가된 아이템 ID를 관리

    private void Awake()
    {
        itemTable = DataTableManager.Get<ItemTable>(DataTableIds.Item);
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
    }

    private void OnEnable()
    {
        RefreshItemSlots();
    }

    private void RefreshItemSlots()
    {
        foreach (var slot in emptySlots)
        {
            Destroy(slot.gameObject);
        }

        emptySlots.Clear();
        itemSlots.Clear();
        addedItems.Clear();

        var activeItemSlots = itemSelectParent.GetComponentsInChildren<ItemSlotUI>();
        foreach (var slot in activeItemSlots)
        {
            if (slot.ItemId != 0)
            {
                addedItems.Add(slot.ItemId);
                itemSlots.Add(slot);
            }
        }

        CreateEmptySlots();
        CreateItemSlots();
    }

    private void CreateEmptySlots()
    {
        for (var i = 0; i < 2; i++)
        {
            var emptySlot = Instantiate(itemSlotPrefab, itemSlotParent);
            emptySlot.Setup(null, null, 0);
            emptySlot.onClickItemSlot = HandleEmptySlotClick;
            emptySlots.Add(emptySlot);
        }
    }


    private void CreateItemSlots()
    {
        var purchasedItems = GameManager.instance.GameData.Items;

        foreach (var purchasedItem in purchasedItems)
        {
            Logger.Log($"Item ID: {purchasedItem.itemId}, Count: {purchasedItem.itemCount}");

            if (!addedItems.Contains(purchasedItem.itemId) &&
                itemTable.table.TryGetValue(purchasedItem.itemId, out var itemData))
            {
                var itemSlot = Instantiate(itemSlotPrefab, itemSelectParent);

                if (assetListTable.table.TryGetValue(itemData.IconNo, out var assetPath))
                {
                    itemSlot.Setup(itemData, assetPath, purchasedItem.itemCount);
                    itemSlot.onClickItemSlot = HandleItemSlotClick;
                    itemSlots.Add(itemSlot);
                    addedItems.Add(purchasedItem.itemId);
                }
            }
        }
    }

    private void HandleItemSlotClick(ItemSlotUI clickedSlot)
    {
        if (clickedSlot == null || clickedSlot.ItemId == 0) return;

        var existingEmptySlot = emptySlots.FirstOrDefault(slot => slot.ItemId == clickedSlot.ItemId);
        if (existingEmptySlot != null)
        {
            return;
        }

        var emptySlot = emptySlots.FirstOrDefault(slot => slot.ItemId == 0);
        if (emptySlot == null)
        {
            return; 
        }

        int limit = clickedSlot.GetOriginalLimit();
        int currentCount = clickedSlot.GetItemCount();

        int addCount = Mathf.Min(currentCount, limit);

        emptySlot.SetItemSlot(clickedSlot.ItemId, clickedSlot.ItemSprite, addCount);

        currentCount -= addCount;
        clickedSlot.UpdateItemCount(currentCount);

        if (currentCount <= 0)
        {
            itemSlots.Remove(clickedSlot);
            addedItems.Remove(clickedSlot.ItemId);
            Destroy(clickedSlot.gameObject);
        }

        SaveItemToLoadTable(clickedSlot.ItemId, addCount);
    }


    private void HandleEmptySlotClick(ItemSlotUI clickedSlot)
    {
        if (clickedSlot == null || clickedSlot.ItemId == 0) return;

        int removeCount = clickedSlot.GetItemCount();

        var originalSlot = itemSlots.FirstOrDefault(slot => slot.ItemId == clickedSlot.ItemId);
        if (originalSlot != null)
        {
            int restoredCount = originalSlot.GetItemCount() + removeCount;
            originalSlot.UpdateItemCount(restoredCount);

            originalSlot.ToggleInteractable(true);
        }
        else
        {
            if (itemTable.table.TryGetValue(clickedSlot.ItemId, out var itemData))
            {
                var newSlot = Instantiate(itemSlotPrefab, itemSelectParent);
                newSlot.Setup(itemData, clickedSlot.ItemSprite.name, removeCount);
                newSlot.onClickItemSlot = HandleItemSlotClick;
                itemSlots.Add(newSlot);
                addedItems.Add(clickedSlot.ItemId);
            }
            else
            {
                Logger.LogError($"ItemData for ItemId {clickedSlot.ItemId} not found.");
            }
        }

        clickedSlot.ClearSlot();
    
        RemoveItemFromLoadTable(clickedSlot.ItemId);
    }

    private void SaveItemToLoadTable(int itemId, int limit)
    {
        Variables.LoadTable.ItemId.Add((itemId, limit));
        Logger.Log($"Saved item {itemId} with limit {limit} to LoadTable");
    }

    private void RemoveItemFromLoadTable(int itemId)
    {
        var itemToRemove = Variables.LoadTable.ItemId.FirstOrDefault(item => item.itemId == itemId);
        if (itemToRemove != default)
        {
            Variables.LoadTable.ItemId.Remove(itemToRemove);

            Logger.Log($"Removed item {itemId} from LoadTable");
        }
    }
}