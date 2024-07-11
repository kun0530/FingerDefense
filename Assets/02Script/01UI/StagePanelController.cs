using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StagePanelController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform stagePanel;
    public float scaleFactor = 0.7f; 
    public float animationDuration = 0.5f; 
    private readonly int[] leftPaddings =
    {
        200,10,-180,-380,-730
    }; 

    private int currentIndex = 0;
    private Vector2 dragStartPosition;
    private HorizontalLayoutGroup layoutGroup;

    void Start()
    {
        layoutGroup = stagePanel.GetComponent<HorizontalLayoutGroup>();
        UpdatePadding();
        UpdateStageSlots();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중에는 아무 작업도 하지 않음
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - dragStartPosition;

        if (dragDelta.x > 0 && currentIndex > 0)
        {
            currentIndex--;
        }
        else if (dragDelta.x < 0 && currentIndex < leftPaddings.Length - 1)
        {
            currentIndex++;
        }

        UpdatePadding();
        UpdateStageSlots();
    }

    private void UpdatePadding()
    {
        if (currentIndex < leftPaddings.Length)
        {
            layoutGroup.padding.left = leftPaddings[currentIndex];
            LayoutRebuilder.ForceRebuildLayoutImmediate(stagePanel); 
        }
    }

    private void UpdateStageSlots()
    {
        for (int i = 0; i < stagePanel.childCount; i++)
        {
            RectTransform rect = stagePanel.GetChild(i).GetComponent<RectTransform>();
            float distanceFromCenter = Mathf.Abs(i - currentIndex);
            float scale = Mathf.Lerp(1.0f, scaleFactor, distanceFromCenter);
            rect.localScale = new Vector3(scale, scale, 1);
            
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0);
        }
    }
}
