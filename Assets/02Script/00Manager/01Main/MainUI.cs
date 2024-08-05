using System;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    private GameManager gameManager;
    
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI ticketText;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("Manager").TryGetComponent(out GameManager manager) ? manager : null;
    }

    private void Update()
    {
        UpdatePlayerName();
    }
    
    public void UpdatePlayerName()
    {
        if (!gameManager)
        {
            Logger.LogError("GameManager is not initialized.");
            return;
        }
        
        if (playerNameText)
        {
            playerNameText.text = gameManager.PlayerName;
        }
        
        if(goldText)
        {
            goldText.text = gameManager.Gold.ToString();
        }
        
        if(diamondText)
        {
            diamondText.text = gameManager.Diamond.ToString();
        }
        
        if(ticketText)
        {
            ticketText.text = gameManager.Ticket.ToString();
        }
        
    }
}
