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

    private int originalLimit; // 선택 전 아이템의 원래 수량

    public void Setup(ItemData item, string assetPath, int count)
    {
        Logger.Log($"Setup called with Item ID: {item?.Id}, Limit: {item?.Limit}");
        ItemId = item?.Id ?? 0;
        originalLimit = item?.Limit ?? 0;
        
        if (item != null && !string.IsNullOrEmpty(assetPath))
        {
            var sprite = Resources.Load<Sprite>($"Prefab/07GameItem/{assetPath}");
            if (sprite != null)
            {
                itemIcon.sprite = sprite;
            }

            itemCount.text = count.ToString();
        }
        else
        {
            itemIcon.sprite = null;
            itemCount.text = "";
        }

        itemButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
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
    }

    public void ToggleInteractable(bool isInteractable)
    {
        itemButton.interactable = isInteractable;
    }

    public int GetOriginalLimit()
    {
        return originalLimit;
    }
}