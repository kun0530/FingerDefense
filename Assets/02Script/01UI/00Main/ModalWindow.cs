using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindow : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;
    public Button ButtonPrefab;
    public Transform buttonParent;

    public static ModalWindow Create()
    {
        var window = Instantiate(Resources.Load<ModalWindow>("Prefab/08Main/ModalWindow"));
        var canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            window.transform.SetParent(canvas.transform, false);
        }
        else
        {
            Debug.LogError("Canvas not found in the scene. Make sure there is a Canvas in the scene.");
        }
        return window;   
    }
    public ModalWindow SetHeader(string header)
    {
        headerText.text = header;
        return this;
    }

    public ModalWindow SetBody(string body)
    {
        bodyText.text = body;
        return this;
    }
    public ModalWindow AddButton(string buttonText, Action onClick)
    {
        var button = Instantiate(ButtonPrefab, buttonParent);
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        button.onClick.AddListener(() =>
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
}
