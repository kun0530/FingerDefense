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

    private List<ItemSlotUI> emptySlots = new List<ItemSlotUI>();  // 빈 슬롯 리스트
    private HashSet<int> addedItems = new HashSet<int>();  // 추가된 아이템 ID를 관리

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
        foreach (var slot in emptySlots)
        {
            Destroy(slot.gameObject);
        }
        emptySlots.Clear();
        addedItems.Clear();

        // 새로운 슬롯 생성
        CreateEmptySlots();
        CreateItemSlots();
    }

    private void CreateEmptySlots()
    {
        for (var i = 0; i < 2; i++) // 빈 슬롯을 2개 생성
        {
            var emptySlot = Instantiate(itemSlotPrefab, itemSlotParent);
            emptySlot.Setup(null, null, 0);
            emptySlot.onClickItemSlot = HandleEmptySlotClick;
            emptySlots.Add(emptySlot);
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
                itemSlot.Setup(item, assetPath, item.Limit);
                itemSlot.onClickItemSlot = clickedSlot => HandleItemSlotClick(clickedSlot);
            }
        }
    }
    
    private void HandleItemSlotClick(ItemSlotUI clickedSlot)
    {
        if (clickedSlot == null) return;

        if (addedItems.Contains(clickedSlot.ItemId))
        {
            Logger.Log("이미 추가된 아이템입니다.");
            return;
        }

        foreach (var emptySlot in emptySlots)
        {
            if (emptySlot.ItemId == 0) // 빈 슬롯에 아이템 배치
            {
                // 클릭된 슬롯의 Limit 값을 가져옴
                int limit = clickedSlot.GetOriginalLimit();
                Logger.Log($"Attempting to add item {clickedSlot.ItemId} with limit {limit}");

                emptySlot.SetItemSlot(clickedSlot.ItemId, clickedSlot.ItemSprite, limit);
                addedItems.Add(clickedSlot.ItemId);
                clickedSlot.ToggleInteractable(false);
                SaveItemToLoadTable(clickedSlot.ItemId, limit);
                break;
            }
        }
    }

    private void HandleEmptySlotClick(ItemSlotUI clickedSlot)
    {
        if (clickedSlot == null || clickedSlot.ItemId == 0) return;

        RemoveItemFromLoadTable(clickedSlot.ItemId);
        addedItems.Remove(clickedSlot.ItemId);
        clickedSlot.ClearSlot();
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
