using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartIconMove : MonoBehaviour
{
    public Image icon;

    public void Start()
    {
        MoveIcon();
    }

    private void MoveIcon()
    {
        RectTransform iconTransform = icon.rectTransform;

        iconTransform.anchoredPosition = new Vector2(302, -80);

        iconTransform.DOAnchorPosY(-60, 1f) 
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo); 
    }
}
