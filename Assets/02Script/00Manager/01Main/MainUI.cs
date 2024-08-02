using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.instance;
        UpdatePlayerName();
    }
    
    public void UpdatePlayerName()
    {
        if (playerNameText != null)
        {
            playerNameText.text = Variables.LoadName.Nickname;    
        }
        
        
    }
}
