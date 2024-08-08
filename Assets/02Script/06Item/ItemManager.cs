using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    // 임시 아이템
    public int item1Id;
    public int item2Id;
    private List<(int itemId, int itemCount)> itemIds = new(); // To-Do: Variables.LoadTable.ItemId로부터 받기

    public int maxItemCount = 2;

    public Transform itemButtonTransform;
    public Button itemButtonPrefab;
    public List<BaseItem> items = new();

    private void Awake()
    {
        // 아이템을 불러온다
        itemIds.Add((item1Id, 3));
        itemIds.Add((item2Id, 3));

        foreach (var itemId in itemIds)
        {
            var item = Resources.Load<BaseItem>($"Items/{itemId.itemId}");
            if (!item)
            {
                Logger.LogError($"해당 아이템 ID에 대응하는 아이템이 존재하지 않습니다: {itemId.itemId}");
                continue;
            }
            item.count = itemId.itemCount;
            items.Add(item);
        }
    }

    private void OnEnable()
    {
        foreach (var item in items)
        {
            if (item && item.IsPassive)
                item.UseItem();
        }
    }

    private void OnDisable()
    {
        foreach (var item in items)
        {
            item.CancelItem();
        }
    }

    private void Start()
    {
        for (int i = 0; i < maxItemCount; i++)
        {
            var itemButton = Instantiate(itemButtonPrefab, itemButtonTransform);
            if (items.Count <= i || !items[i])
            {
                itemButton.interactable = false;
                continue;
            }

            // To-Do: 아이템 이미지 넣기

            if (items[i].IsPassive)
            {
                itemButton.interactable = false;
                continue;
            }

            items[i].button = itemButton;
            int index = i;
            itemButton.onClick.AddListener(items[index].UseItem);
            itemButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{items[i].name}";
        }
    }

    private void Update()
    {
        foreach (var item in items)
        {
            item?.UpdateItem();
        }
    }
}
