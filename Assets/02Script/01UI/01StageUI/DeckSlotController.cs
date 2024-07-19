using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSlotController : MonoBehaviour
{
    public RectTransform characterSlotParent;
    public RectTransform filterringSlotParent;

    private PlayerCharacterTable playerCharacterTable;
    public CharacterSlotUI characterSlotPrefab; // 캐릭터 UI를 생성하기 위한 프리팹

    private List<CharacterSlotUI> characterSlots = new List<CharacterSlotUI>();
    private List<CharacterSlotUI> filterSlots = new List<CharacterSlotUI>();
    private HashSet<int> addedCharacters = new HashSet<int>();
    private List<CharacterSlotUI> activeChoicePanelSlots = new List<CharacterSlotUI>();
    
    public Button startButton;
    public Button AttributeFilterButton;
    
    private void Start()
    {
        playerCharacterTable ??= DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);

        CreateCharacterSlots();
        CreateFilteringSlots();
        
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(2);
        });
    }

    private void CreateCharacterSlots()
    {
        for (var i = 0; i < 8; i++)
        {
            AddEmptyCharacterSlot();
        }
    }

    private void CreateFilteringSlots()
    {
        var characters = playerCharacterTable.GetAll();
        foreach (var characterData in characters)
        {
            var characterSlot = Instantiate(characterSlotPrefab, filterringSlotParent);
            characterSlot.SetCharacterSlot(characterData);
            characterSlot.OnSlotClick = HandleCharacterSlotClick;
            filterSlots.Add(characterSlot);
        }
    }

    private void AddEmptyCharacterSlot()
    {
        var slot = Instantiate(characterSlotPrefab, characterSlotParent);
        slot.ChoicePanel.SetActive(false);
        slot.OnSlotClick = HandleCharacterSlotClick;
        characterSlots.Add(slot);
    }

    public void HandleCharacterSlotClick(CharacterSlotUI clickedSlot)
    {
        if (clickedSlot.transform.parent == filterringSlotParent)
        {
            if (addedCharacters.Contains(clickedSlot.characterData.Id))
            {
                Logger.Log("이미 추가된 캐릭터입니다.");
                return;
            }
            
            if(addedCharacters.Count >= 8)
            {
                Logger.Log("최대 8개까지만 추가 가능합니다.");
                return;
            }

            // 필터링 슬롯에서 클릭됨
            foreach (var slot in characterSlots)
            {
                if (slot.characterData == null)
                {
                    slot.SetCharacterSlot(clickedSlot.characterData);
                    slot.ChoicePanel.SetActive(false); // characterSlotParent에 추가된 슬롯은 ChoicePanel 비활성화
                    clickedSlot.ChoiceButton.interactable = false; // 원본 슬롯의 버튼을 비활성화
                    addedCharacters.Add(clickedSlot.characterData.Id);
                    activeChoicePanelSlots.Add(clickedSlot);
                    UpdateChoicePanels();
                    
                    break;
                }
            }
            SortCharacterSlots();
        }
        else if (clickedSlot.transform.parent == characterSlotParent)
        {
            // 캐릭터 슬롯에서 클릭됨
            var originalSlot = activeChoicePanelSlots.Find(slot => slot.characterData == clickedSlot.characterData);
            if (originalSlot != null)
            {
                originalSlot.ChoicePanel.SetActive(true);
                originalSlot.ChoiceButton.interactable = true;
                activeChoicePanelSlots.Remove(originalSlot);
                addedCharacters.Remove(originalSlot.characterData.Id);
                UpdateChoicePanels();
            }

            clickedSlot.ClearSlot();
            characterSlots.Remove(clickedSlot);
            Destroy(clickedSlot.gameObject);
            AddEmptyCharacterSlot();
            
            SortCharacterSlots();
        }
    }

    private void SortCharacterSlots()
    {
        // Priority > Grade > Element 순서대로 정렬
        characterSlots = characterSlots
            .OrderBy(slot => slot.characterData?.Priority ?? int.MaxValue)
            .ThenByDescending(slot => slot.characterData?.Grade ?? int.MinValue)
            .ThenByDescending(slot => slot.characterData?.Element ?? int.MinValue)
            .ToList();
    
        // 정렬된 캐릭터 슬롯을 로그로 출력하여 확인
        foreach (var slot in characterSlots)
        {
            if (slot.characterData != null)
            {
                Debug.Log($"Priority: {slot.characterData.Priority}, Grade: {slot.characterData.Grade}, Element: {slot.characterData.Element}");
            }
            else
            {
                Debug.Log("빈 슬롯");
            }
        }
    
        // UI 요소를 정렬된 순서에 맞게 재배치
        for (var i = 0; i < characterSlots.Count; i++)
        {
            characterSlots[i].transform.SetSiblingIndex(i);
        }
    }

    private void UpdateChoicePanels()
    {
        foreach (var slot in filterSlots)
        {
            slot.ChoicePanel.SetActive(false);
        }

        foreach (var slot in activeChoicePanelSlots)
        {
            slot.ChoicePanel.SetActive(true);
        }
    }
}
