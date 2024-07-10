using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class StagePanelController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public StageSlot[] stageSlots;
    public RectTransform stagePanel;
    public float scaleFactor = 0.7f; // 가운데가 아닌 슬라이드의 크기 비율
    public float spacing = 200f; // 각 StageSlot 간의 간격
    public float animationDuration = 0.5f; // 애니메이션 지속 시간

    private Vector2 dragStartPosition;
    private Vector2 panelStartPosition;
    private int currentIndex = 0;

    void Start()
    {
        UpdateStageSlots();
    }

    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
        panelStartPosition = stageSlots[0].GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - dragStartPosition;
        foreach (StageSlot slot in stageSlots)
        {
            RectTransform rect = slot.GetComponent<RectTransform>();
            rect.anchoredPosition = panelStartPosition + new Vector2(dragDelta.x, 0);
        }

        UpdateStageSlots();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float closestDistance = float.MaxValue;
        for (int i = 0; i < stageSlots.Length; i++)
        {
            float distance = Mathf.Abs(stageSlots[i].GetComponent<RectTransform>().anchoredPosition.x - panelStartPosition.x);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentIndex = i;
            }
        }
        SnapToSlot(currentIndex);
    }

    private void SnapToSlot(int index)
    {
        Vector2 targetPosition = new Vector2(-index * spacing, panelStartPosition.y);
        foreach (StageSlot slot in stageSlots)
        {
            RectTransform rect = slot.GetComponent<RectTransform>();
            rect.DOAnchorPos(targetPosition + new Vector2((slot.transform.GetSiblingIndex() - index) * spacing, 0), animationDuration).OnComplete(UpdateStageSlots);
        }
    }

    private void UpdateStageSlots()
    {
        for (int i = 0; i < stageSlots.Length; i++)
        {
            float distanceFromCenter = Mathf.Abs(i - currentIndex);
            float scale = Mathf.Lerp(1.0f, scaleFactor, distanceFromCenter);
            stageSlots[i].transform.localScale = new Vector3(scale, scale, 1);

            float positionX = panelStartPosition.x + (i - currentIndex) * spacing;
            stageSlots[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(positionX, panelStartPosition.y);
        }
    }
}
