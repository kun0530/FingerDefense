using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 임시 아이템
    private int item1Id;
    private int item2Id;

    // To-Do: 변수명 변경
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
            if (item && item.IsPassive)
                item.CancelItem();
        }
    }

    private void Update()
    {
        foreach (var item in items)
        {
            item?.UpdateItem();
        }
    }

    private void OnGUI()
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.IsPassive)
                continue;

            if (GUILayout.Button($"Item{count++}"))
            {
                item?.UseItem();
            }
        }
    }
}
