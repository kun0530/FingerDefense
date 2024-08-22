using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private ItemTable itemTable;

    public int maxItemCount = 2;

    public Transform itemButtonTransform;
    public UiSlotButton itemButtonPrefab;
    private List<BaseItem> items = new();

    private void Awake()
    {
        SetItemList();
        CreateItemButton();
    }

    private void OnEnable()
    {
        foreach (var item in items)
        {
            item.Init();
            if (item && item.IsPassive)
            {
                item.UseItem();
                item.button.buttonEffect.gameObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var item in items)
        {
            item?.CancelItem();
        }
    }

    private void Update()
    {
        foreach (var item in items)
        {
            item?.UpdateItem();
        }
    }

    private void SetItemList()
    {
        itemTable = DataTableManager.Get<ItemTable>(DataTableIds.Item);
        var itemIds = Variables.LoadTable.ItemId;
        for (int i = 0; i < maxItemCount && i < itemIds.Count; i++)
        {
            var itemId = itemIds[i];
            var itemData = itemTable.Get(itemId.itemId);
            if (itemData == null)
            {
                Logger.LogError($"해당 아이템 ID에 대응하는 아이템이 아이템 테이블에 존재하지 않습니다: {itemId.itemId}");
                continue;
            }
            var item = Resources.Load<BaseItem>($"Items/{itemData.DataName}");
            if (!item)
            {
                Logger.LogError($"해당 아이템 ID에 대응하는 아이템이 폴더에 존재하지 않습니다: {itemId.itemId}");
                continue;
            }
            item.id = itemId.itemId;
            item.count = itemId.itemCount;
            items.Add(item);
        }
    }

    private void CreateItemButton()
    {
        var assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        var stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);

        for (int i = 0; i < maxItemCount; i++)
        {
            var itemButton = Instantiate(itemButtonPrefab, itemButtonTransform);
            if (items.Count <= i || !items[i])
            {
                itemButton.ActiveButton(false);
                continue;
            }
            var itemData = itemTable.Get(items[i].id);

            var itemName = stringTable.Get(itemData.NameId.ToString());
            itemButton.text.text = itemName;

            var itemFilePath = assetListTable.Get(itemData.IconNo);
            var itemImage = Resources.Load<Sprite>($"Prefab/07GameItem/{itemFilePath}");
            if (itemImage)
                itemButton.GetComponent<UiSlotButton>().slotImage.sprite = itemImage;
            
            items[i].button = itemButton;

            if (items[i].IsPassive || items[i].count <= 0)
            {
                itemButton.ActiveButton(false);
                continue;
            }

            int index = i;
            itemButton.button.onClick.AddListener(items[index].UseItem);
            itemButton.ActiveButton(true);
        }
    }
}
