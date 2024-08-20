using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class StagePanelController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform stagePanel;
    public float scaleFactor = 0.7f;
    public float animationDuration = 0.2f;
    private readonly int[] leftPaddings = { 200,-100,-375,-650,-1100 };

    private int currentIndex = 0;
    private Vector2 dragStartPosition;
    private HorizontalLayoutGroup layoutGroup;

    public TutorialController stageTutorial;
    public TutorialController DeckTutorial;
    public TutorialController SpecialDragTutorial;
    
    private void Start()
    {
        layoutGroup = stagePanel.GetComponent<HorizontalLayoutGroup>();

        if (stageTutorial.gameObject.activeSelf)
        {
            currentIndex = 0;           
        }
        else if (SpecialDragTutorial.gameObject.activeSelf)
        {
            currentIndex = 2;
        }
        else if (DeckTutorial.gameObject.activeSelf)
        {
            currentIndex = 1;
        }    
        
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
        if (stagePanel.childCount <= 1)
        {
            return;
        }
        
        Vector2 dragDelta = eventData.position - dragStartPosition;
        float minDragDistance = 50.0f;
        if (dragDelta.x > minDragDistance && currentIndex > 0)
        {
            currentIndex--;
        }
        else if (dragDelta.x < -minDragDistance && currentIndex < leftPaddings.Length - 1)
        {
            currentIndex++;
        }

        UpdatePadding();
        UpdateStageSlots();
    }

    private void UpdatePadding(bool instant = false)
    {
        if (stagePanel.childCount <= 1)
        {
            layoutGroup.padding.left = 0;
            LayoutRebuilder.ForceRebuildLayoutImmediate(stagePanel);
            return;
        }
        
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

    // private void UpdateStageSlots(bool instant = false)
    // {
    //     for (var i = 0; i < stagePanel.childCount; i++)
    //     {
    //         var rect = stagePanel.GetChild(i).GetComponent<RectTransform>();
    //         float distanceFromCenter = Mathf.Abs(i - currentIndex);
    //         var scale = Mathf.Lerp(1.0f, scaleFactor, distanceFromCenter);
    //
    //         if (instant)
    //         {
    //             rect.localScale = new Vector3(scale, scale, 1);
    //         }
    //         else
    //         {
    //             rect.DOScale(new Vector3(scale, scale, 1), animationDuration).SetEase(Ease.InOutQuad);
    //         }
    //         var isInteractable = (distanceFromCenter < 1.0f);
    //         SetButtonsInteractable(rect, isInteractable);
    //         
    //         rect.anchorMin = new Vector2(0.5f, 0.5f);
    //         rect.anchorMax = new Vector2(0.5f, 0.5f);
    //         rect.pivot = new Vector2(0.5f, 0.5f);
    //         rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0); 
    //     }
    //
    //     return;
    //
    //     void SetButtonsInteractable(Transform slot, bool isInteractable)
    //     {
    //         var buttons = slot.GetComponentsInChildren<Button>();
    //         foreach (var button in buttons)
    //         {
    //             button.interactable = isInteractable;
    //         }
    //     }
    // }
    private void UpdateStageSlots(bool instant = false)
    {
        for (var i = 0; i < stagePanel.childCount; i++)
        {
            var rect = stagePanel.GetChild(i).GetComponent<RectTransform>();
            float distanceFromCenter = Mathf.Abs(i - currentIndex); // 현재 인덱스와 각 슬롯의 인덱스 간의 거리 계산
            var scale = Mathf.Lerp(scaleFactor, 1.0f, 1.0f - Mathf.Clamp01(distanceFromCenter)); // 거리 기반 스케일 계산

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

            // 앵커와 피벗을 중앙으로 설정
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
