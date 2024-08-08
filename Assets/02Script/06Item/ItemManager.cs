using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    // 임시 아이템
    private int item1Id;
    private int item2Id;

    public int maxItemCount = 2;

    public Transform itemButtonTransform;
    public Button itemButtonPrefab;
    public List<BaseItem> items = new();

    private void Awake()
    {
        // 아이템을 불러온다
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
            if (items.Count <= i)
            {
                itemButton.interactable = false;
                continue;
            }

            // 아이템 이미지 넣기
            if (items[i].IsPassive)
            {
                itemButton.interactable = false;
                continue;
            }

            items[i].button = itemButton;
            int index = i;
            itemButton.onClick.AddListener( () => 
                {
                    if (items[index].UseItem())
                        itemButton.interactable = false;
                }
            );
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
