using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSlotButton : MonoBehaviour
{
    public Image backgroundBlack;
    public Image slotImage;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetFillAmountBackground(float amount)
    {
        amount = Mathf.Clamp01(amount);
        backgroundBlack.fillAmount = amount;
    }
}
