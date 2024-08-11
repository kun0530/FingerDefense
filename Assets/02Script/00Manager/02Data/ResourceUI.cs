using System;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour, IResourceObserver
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diamondText;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ResourceManager.RegisterObserver(this);    
        }
        
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ResourceManager.RemoveObserver(this);
        }
    }

    public void OnResourceUpdate(ResourceType resourceType, int newValue)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                goldText.text = $"Gold: {newValue}";
                break;
            case ResourceType.Diamond:
                diamondText.text = $"Diamond: {newValue}";
                break;
            // 다른 리소스도 필요시 추가
        }
    }

    private void UpdateUI()
    {
        OnResourceUpdate(ResourceType.Gold, GameManager.instance.ResourceManager.Gold);
        OnResourceUpdate(ResourceType.Diamond, GameManager.instance.ResourceManager.Diamond);
    }
}