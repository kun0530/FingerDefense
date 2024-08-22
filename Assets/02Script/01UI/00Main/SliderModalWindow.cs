using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderModalWindow : MonoBehaviour
{
    public Transform buttonParent;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    public Button buyButton;
    public Slider countSlider;
    private StringTable stringTable;
    private GameObject modalMask;
    private int baseCost;

    
    private void OnEnable()
    {
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
        countSlider.onValueChanged.AddListener(OnSliderValueChanged);
        countSlider.minValue = 1;
        countSlider.maxValue = 99;
        modalMask= GameObject.FindWithTag("ModalMask");    
    }

    private void OnSliderValueChanged(float count)
    {
        // 원래의 cost 값에 슬라이더 값을 곱한 결과를 표시
        costText.text = (baseCost * (int)count).ToString(CultureInfo.InvariantCulture);
        bodyText.text = $"{(int)count}개";
    }

    public static SliderModalWindow Create()
    {
        var window = Instantiate(Resources.Load<SliderModalWindow>("Prefab/08Main/SliderModalWindow"));
        var canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            window.transform.SetParent(canvas.transform, false);
        }
        else
        {
            Logger.LogError("Canvas not found in the scene. Make sure there is a Canvas in the scene.");
        }
        return window;   
    }
    
    public SliderModalWindow SetHeader(int headerId)
    {
        var headerValue = stringTable.Get(headerId.ToString());
        if (string.IsNullOrEmpty(headerValue))
        {
            Debug.LogError($"Header ID {headerId}에 해당하는 텍스트를 찾을 수 없습니다.");
        }
        else
        {
            headerText.text = headerValue;
        }
        return this;
    }
    
    public SliderModalWindow SetHeader(string header)
    {
        if (string.IsNullOrEmpty(header))
        {
            Debug.LogError("Header text가 빈 문자열이거나 null입니다.");
        }
        else
        {
            headerText.text = header;
        }
        return this;
    }
    
    
    public SliderModalWindow SetBody(int body)
    {
        bodyText.text = $"{body}개";
        return this;
    }
    
    public SliderModalWindow SetCost(int cost)
    {
        baseCost = cost;
        costText.text = cost.ToString();
        return this;
    }
    
    public SliderModalWindow SetDescription(int descriptionId)
    {
        descriptionText.text = stringTable.Get(descriptionId.ToString());
        return this;
    }
    
    public SliderModalWindow SetDescription(string description)
    {
        descriptionText.text = description;
        return this;
    }
    
    public SliderModalWindow AddButton(int buttonId, Action onClick)
    {
        buyButton.GetComponentInChildren<TextMeshProUGUI>().text = stringTable.Get(buttonId.ToString());
        buyButton.onClick.AddListener(() =>
        {
            onClick?.Invoke();
            Destroy(gameObject);
            modalMask.SetActive(false);
            modalMask.transform.SetAsFirstSibling();
        });
        return this;
    }
    
    public SliderModalWindow AddButton(string buttonText, Action onClick)
    {
        var buy = Instantiate(buyButton, buttonParent);
        buy.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        buy.onClick.AddListener(() =>
        {
            onClick?.Invoke();
            Destroy(gameObject);
        });
        return this;
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        
    }
    public static float GetCurrentSliderValue()
    {
        return FindObjectOfType<SliderModalWindow>().countSlider.value;
    }
    
}
