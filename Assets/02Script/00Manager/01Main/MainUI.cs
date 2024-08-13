using System;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks; 

public class MainUI : MonoBehaviour, IResourceObserver
{
    public TextMeshProUGUI playerNameText;
    private GameManager gameManager;
    private ResourceManager resourceManager;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI ticketText;

    private void Start()
    {
        gameManager = GameManager.instance;
        resourceManager = GameObject.FindWithTag("Resources").TryGetComponent(out ResourceManager manager) ? manager : null;
        
        gameManager.ResourceManager.RegisterObserver(this);
        UpdatePlayerInfo();
    }

    private void OnDestroy()
    {
        if (gameManager != null && gameManager.ResourceManager != null)
        {
            gameManager.ResourceManager.RemoveObserver(this);
        }
    }

    public void OnResourceUpdate(ResourceType resourceType, int newValue)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                goldText.text = newValue.ToString();
                break;
            case ResourceType.Diamond:
                diamondText.text = newValue.ToString();
                break;
            case ResourceType.Ticket:
                ticketText.text = newValue.ToString();
                break;
        }
    }

    public void UpdatePlayerInfo()
    {
        if (playerNameText)
        {
            playerNameText.text = gameManager.ResourceManager.PlayerName;
        }
        
        if (goldText)
        {
            goldText.text = gameManager.ResourceManager!.Gold.ToString();
        }

        if (diamondText)
        {
            diamondText.text = gameManager.ResourceManager!.Diamond.ToString();
        }

        if (ticketText)
        {
            ticketText.text = gameManager.ResourceManager!.Ticket.ToString();
        }
    }
}
