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
    public Button closeButton;
    
    private void Start()
    {
        playerCharacterTable ??= DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);

        CreateCharacterSlots();
        CreateFilteringSlots();
        
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(2);
        });
        closeButton.onClick.AddListener(() =>
        {
            //할당 되어있는 값을 초기화 시킨다.
            Defines.LoadTable.stageId = 0;
            Logger.Log("스테이지 선택 화면으로 이동합니다.");
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

    private void HandleCharacterSlotClick(CharacterSlotUI clickedSlot)
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

            foreach (var slot in characterSlots)
            {
                if (slot.characterData == null)
                {
                    slot.SetCharacterSlot(clickedSlot.characterData);
                    slot.ChoicePanel.SetActive(false);
                    clickedSlot.ChoiceButton.interactable = false;
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

    public void UpdateFilteredSlots(List<PlayerCharacterData> filteredCharacters)
    {
        foreach (var slot in filterSlots)
        {
            if (filteredCharacters.Contains(slot.characterData))
            {
                slot.gameObject.SetActive(true);
                Logger.Log($"Character ID: {slot.characterData.Id} is visible.");
            }
            else
            {
                slot.gameObject.SetActive(false);
                Logger.Log($"Character ID: {slot.characterData.Id} is hidden.");
            }
        }
    }

    private void SortCharacterSlots()
    {
        characterSlots = characterSlots
            .OrderBy(slot => slot.characterData?.Class ?? int.MaxValue)
            .ThenByDescending(slot => slot.characterData?.Grade ?? int.MinValue)
            .ThenByDescending(slot => slot.characterData?.Element ?? int.MinValue)
            .ToList();
    
        foreach (var slot in characterSlots)
        {
            Logger.Log(slot.characterData != null
                ? $"Priority: {slot.characterData.Class}, Grade: {slot.characterData.Grade}, Element: {slot.characterData.Element}"
                : "빈 슬롯");
        }
    
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
