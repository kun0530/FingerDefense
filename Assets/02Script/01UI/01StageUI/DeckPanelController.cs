using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckPanelController : MonoBehaviour
{
    public Button changeButton;

    public RectTransform CharacterPanel;
    public RectTransform DeckPanel;
    public Button CharacterFilterButton;
    
    private bool isCharacterPanelActive = true;
    
    public void Start()
    {
        changeButton.onClick.AddListener(ChangePanel);
        changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "<b>캐릭터</b>/아이템";
    }
    private void ChangePanel()
    {
        isCharacterPanelActive = !isCharacterPanelActive;

        if (isCharacterPanelActive)
        {
            DeckPanel.gameObject.SetActive(false);
            CharacterPanel.gameObject.SetActive(true);
            CharacterPanel.SetAsLastSibling();
            
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "<b>캐릭터</b>/아이템";
        }
        else
        {
            CharacterPanel.gameObject.SetActive(false);
            DeckPanel.SetAsLastSibling();
            DeckPanel.gameObject.SetActive(true);
            
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "캐릭터/<b>아이템</b>";
        }

        CharacterFilterButton.gameObject.SetActive(isCharacterPanelActive);
    }
    
}
