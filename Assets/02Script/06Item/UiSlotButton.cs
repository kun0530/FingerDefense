using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSlotButton : MonoBehaviour
{
    public Button button;
    public Image backgroundBlack;
    public Image slotImage;
    public TextMeshProUGUI text;

    public void SetFillAmountBackground(float amount)
    {
        amount = Mathf.Clamp01(amount);
        backgroundBlack.fillAmount = amount;

        if (amount == 0f)
            button.interactable = true;
        else if (amount == 1f)
            button.interactable = false;
    }

    public void ActiveButton(bool isActive)
    {
        SetFillAmountBackground(isActive ? 0f : 1f);
    }
}
