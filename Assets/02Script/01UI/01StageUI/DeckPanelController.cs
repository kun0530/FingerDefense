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
    }
    private void ChangePanel()
    {
        isCharacterPanelActive = !isCharacterPanelActive;

        if (isCharacterPanelActive)
        {
            CharacterPanel.SetAsLastSibling();
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "아이템";
        }
        else
        {
            DeckPanel.SetAsLastSibling();
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "캐릭터";
        }

        CharacterFilterButton.gameObject.SetActive(isCharacterPanelActive);
    }
    
}
