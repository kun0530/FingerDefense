using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 임시 아이템
    private int item1Id;
    private int item2Id;

    // To-Do: 변수명 변경
    public BaseItem item1;
    public BaseItem item2;

    private void Awake()
    {
        // 아이템을 불러온다
    }

    private void OnEnable()
    {
        if (item1 && item1.IsPassive)
            item1.UseItem();
        if (item2 && item2.IsPassive)
            item2.UseItem();
    }

    private void OnDisable()
    {
        if (item1 && item1.IsPassive)
            item1.CancelItem();
        if (item2 && item2.IsPassive)
            item2.CancelItem();
    }

    private void Update()
    {
        // 아이템 중 액티브의 쿨타임을 확인한다..
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Item1"))
        {
            item1?.UseItem();
        }
        if (GUILayout.Button("Item2"))
        {
            item2?.UseItem();
        }
    }
}
