using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemCount;
    public Button itemButton;
    public RectTransform ChoicePanel;

    public delegate void OnClickItemSlot(ItemSlotUI itemSlot);
    public OnClickItemSlot onClickItemSlot;

    public int ItemId { get; private set; }
    public Sprite ItemSprite => itemIcon.sprite;

    private int originalLimit; 

    public void Setup(ItemData item, string assetPath, int count)
    {
        Logger.Log($"Setup called with Item ID: {item?.Id}, Count: {count}");
        ItemId = item?.Id ?? 0;

        if (item != null && !string.IsNullOrEmpty(assetPath))
        {
            var sprite = Resources.Load<Sprite>($"Prefab/07GameItem/{assetPath}");
            if (sprite != null)
            {
                itemIcon.sprite = sprite;
            }
            originalLimit = item.Limit;
            itemCount.text = count.ToString();
        }
        else
        {
            itemIcon.sprite = null;
            itemCount.text = "";
        }

        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() =>
        {
            onClickItemSlot?.Invoke(this);
        });
    }

    public void SetItemSlot(int itemId, Sprite sprite, int count)
    {
        ItemId = itemId;
        itemIcon.sprite = sprite;
        itemCount.text = count.ToString();
        
        
        ChoicePanel.SetAsLastSibling();
    }

    public void ClearSlot()
    {
        ItemId = 0;
        itemIcon.sprite = null;
        itemCount.text = "";
        ChoicePanel.gameObject.SetActive(false);
        ToggleInteractable(true);
    }

    public void ToggleInteractable(bool isInteractable)
    {
        itemButton.interactable = isInteractable;
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
    
}