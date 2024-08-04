using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class StagePanelController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform stagePanel;
    public float scaleFactor = 0.7f;
    public float animationDuration = 0.5f;
    private readonly int[] leftPaddings = { 200,-100,-375,-650,-1100 };

    private int currentIndex = 0;
    private Vector2 dragStartPosition;
    private HorizontalLayoutGroup layoutGroup;

    private void Start()
    {
        layoutGroup = stagePanel.GetComponent<HorizontalLayoutGroup>();
        
        UpdatePadding(true); 
        UpdateStageSlots(true); 
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - dragStartPosition;

        switch (dragDelta.x)
        {
            case > 0 when currentIndex > 0:
                currentIndex--;
                break;
            case < 0 when currentIndex < leftPaddings.Length - 1:
                currentIndex++;
                break;
        }

        UpdatePadding();
        UpdateStageSlots();
    }

    private void UpdatePadding(bool instant = false)
    {
        if (currentIndex < leftPaddings.Length)
        {
            if (instant)
            {
                layoutGroup.padding.left = leftPaddings[currentIndex];
                LayoutRebuilder.ForceRebuildLayoutImmediate(stagePanel);
            }
            else
            {
                DOTween.To(() => layoutGroup.padding.left, x => layoutGroup.padding.left = x, leftPaddings[currentIndex], animationDuration)
                    .SetEase(Ease.InOutQuad)
                    .OnUpdate(() =>
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(stagePanel);
                    });
            }
        }
    }

    private void UpdateStageSlots(bool instant = false)
    {
        for (var i = 0; i < stagePanel.childCount; i++)
        {
            var rect = stagePanel.GetChild(i).GetComponent<RectTransform>();
            float distanceFromCenter = Mathf.Abs(i - currentIndex);
            var scale = Mathf.Lerp(1.0f, scaleFactor, distanceFromCenter);

            if (instant)
            {
                rect.localScale = new Vector3(scale, scale, 1);
            }
            else
            {
                rect.DOScale(new Vector3(scale, scale, 1), animationDuration).SetEase(Ease.InOutQuad);
            }
            var isInteractable = (distanceFromCenter < 1.0f);
            SetButtonsInteractable(rect, isInteractable);
            
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0); 
        }

        return;

        void SetButtonsInteractable(Transform slot, bool isInteractable)
        {
            var buttons = slot.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                button.interactable = isInteractable;
            }
        }
    }
}
