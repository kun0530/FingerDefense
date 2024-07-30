using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNameText;
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.instance;
        UpdatePlayerName();
    }
    
    public void UpdatePlayerName()
    {
        playerNameText.text =gameManager.PlayerName;
    }
}
