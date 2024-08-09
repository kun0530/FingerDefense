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
    
    
    public void Start()
    {
        changeButton.onClick.AddListener(ChangePanel);
    }
    
    //changeButton을 누르면 CharacterPanel과 DeckPanel의 활성화 여부를 변경한다.CharacterFilterButton도 CharacterPanel의 활성화 여부에 따라서 활성화 여부를 변경한다.
    //활성화 된 패널을 맨 뒤로 보낸다 (SetAsLastSibling)
    private void ChangePanel()
    {
        bool isCharacterPanelActive = !CharacterPanel.gameObject.activeSelf;
        
        // CharacterPanel의 활성화 여부를 설정한다.
        CharacterPanel.gameObject.SetActive(isCharacterPanelActive);
        
        // DeckPanel의 활성화 여부를 CharacterPanel의 반대로 설정한다.
        DeckPanel.gameObject.SetActive(!isCharacterPanelActive);
        
        // CharacterFilterButton의 활성화 여부를 CharacterPanel의 활성화 여부에 맞춘다.
        CharacterFilterButton.gameObject.SetActive(isCharacterPanelActive);
        if (CharacterPanel.gameObject.activeSelf)
        {
            CharacterPanel.SetAsLastSibling();
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "캐릭터";
        }
        else
        {
            DeckPanel.SetAsLastSibling();
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "아이템";
        }
    }
    
}
