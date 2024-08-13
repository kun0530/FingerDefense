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
        // CharacterPanel과 DeckPanel이 항상 활성화된 상태를 유지하도록 함
        CharacterPanel.gameObject.SetActive(true);
        DeckPanel.gameObject.SetActive(true);
    
        bool isCharacterPanelActive = !CharacterPanel.GetSiblingIndex().Equals(CharacterPanel.parent.childCount - 1);

        if (isCharacterPanelActive)
        {
            // CharacterPanel을 맨 앞으로 보냄
            CharacterPanel.SetAsLastSibling();
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "캐릭터";
        }
        else
        {
            // DeckPanel을 맨 앞으로 보냄
            DeckPanel.SetAsLastSibling();
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "아이템";
        }

        // CharacterFilterButton의 활성화 여부를 CharacterPanel의 맨 앞 여부에 맞춤
        CharacterFilterButton.gameObject.SetActive(isCharacterPanelActive);
    }
    
}
