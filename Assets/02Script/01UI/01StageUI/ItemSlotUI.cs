using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemCount;
    
    public Button itemButton;
    
    public delegate void OnClickItemSlot(ItemSlotUI itemSlot);
    public OnClickItemSlot onClickItemSlot;
    
    public RectTransform ChoicePanel;

    private int itemId; // 현재 슬롯에 설정된 아이템의 ID
    
    public void Setup(ItemData item, string assetPath, int count)
    {
        if (item != null && !string.IsNullOrEmpty(assetPath))
        {
            itemId = item.Id;

            var sprite = Resources.Load<Sprite>($"Prefab/07GameItem/{assetPath}");
            if (sprite != null)
            {
                itemIcon.sprite = sprite;
            }

            // Variables.LoadTable.ItemId에서 가져온 실제 아이템 개수를 표시
            itemCount.text = count.ToString();
        }
        else
        {
            itemIcon.sprite = null;
            itemCount.text = "";
        }
        ChoicePanel.transform.SetAsLastSibling();
        itemButton.onClick.AddListener(() =>
        {
            onClickItemSlot?.Invoke(this);
            UpdateItemCount();
        });
    }

    private void UpdateItemCount()
    {
        // Variables.LoadTable.ItemId에 해당 아이템이 있는지 확인
        var existingItem = Variables.LoadTable.ItemId.FirstOrDefault(tuple => tuple.Item1 == itemId);

        if (existingItem != null)
        {
            // 기존 아이템의 개수를 99로 설정
            Variables.LoadTable.ItemId.Remove(existingItem);
        }
        
        Variables.LoadTable.ItemId.Add(new Tuple<int, int>(itemId, 99));

        // UI에 변경된 개수를 표시
        itemCount.text = "99";
    }
}