using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemSlotUI : MonoBehaviour,IPointerUpHandler, IPointerDownHandler
{
    public Image itemIcon;
    public TextMeshProUGUI itemCount;
    public RectTransform ChoicePanel;

    public delegate void OnClickItemSlot(ItemSlotUI itemSlot);
    public OnClickItemSlot onClickItemSlot;

    public int ItemId { get; private set; }
    public Sprite ItemSprite => itemIcon.sprite;

    private int originalLimit;
    private bool isPressed = false;
    private bool isLongPress = false;

    public Action<ItemSlotUI> OnLongPress;
    public Action OnLongPressRelease;
    
    public async UniTaskVoid Setup(ItemData item, string assetPath, int count)
    {
        Logger.Log($"Setup called with Item ID: {item?.Id}, Count: {count}");
        ItemId = item?.Id ?? 0;

        if (item != null && !string.IsNullOrEmpty(assetPath))
        {
            var handle = Addressables.LoadAssetAsync<Sprite>($"Prefab/07GameItem/{assetPath}");
            Sprite sprite = await handle.ToUniTask(); // UniTask로 변환하여 await

            if (sprite != null)
            {
                itemIcon.sprite = sprite;
                var color = itemIcon.color;
                color.a = 1f;
                itemIcon.color = color;
            }
            originalLimit = item.Limit;
            itemCount.text = count.ToString();
        }
        else
        {
            itemIcon.sprite = null;
            itemCount.text = "";
        }
    }

    public void SetItemSlot(int itemId, Sprite sprite, int count)
    {
        ItemId = itemId;
        itemIcon.sprite = sprite;
        itemCount.text = count.ToString();
        var color = itemIcon.color;
        color.a = 1f; 
        itemIcon.color = color;
        
        ChoicePanel.SetAsLastSibling();
    }

    public void ClearSlot()
    {
        ItemId = 0;
        itemIcon.sprite = null;
        itemCount.text = "";
        var color = itemIcon.color;
        color.a = 0f;
        itemIcon.color = color;
        ChoicePanel.gameObject.SetActive(false);
    }

    public int GetOriginalLimit()
    {
        return originalLimit;
    }

    public int GetItemCount()
    {
        return itemCount.text == "" ? 0 : int.Parse(itemCount.text);
    }

    public void UpdateItemCount(int currentCount)
    {
        Logger.Log($"Updating item count to: {currentCount} for Item ID: {ItemId}");
        itemCount.text = currentCount.ToString();
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (isLongPress)
        {
            OnLongPressRelease?.Invoke();
        }
        else
        {
            // 일반 터치 시 슬롯 클릭 처리
            onClickItemSlot?.Invoke(this);
        }

        isLongPress = false;    
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        isLongPress = false;

        PressRoutine().Forget(); // PressRoutine 메서드 비동기 실행    
    }
    
    private async UniTaskVoid PressRoutine()
    {
        await UniTask.Delay(500); // 500ms 이상 누르면 롱터치로 인식
        if (isPressed)
        {
            isLongPress = true;
            OnLongPress?.Invoke(this); // 롱터치 시 설명창만 활성화
        }
    }
}