using System;
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
    
    public GameManager gameManager;
    
    private void Awake()
    {
        playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        RefreshCharacterSlots();
        
        startButton.onClick.AddListener(() =>
        {
            SaveCharacterSelection(); // 캐릭터 선택 상태 저장
            SceneManager.LoadScene(2);
        });
        
        closeButton.onClick.AddListener(() =>
        {
            // 할당 되어있는 값을 초기화 시킨다.
            Variables.LoadTable.StageId = 0;
            Logger.Log("스테이지 선택 화면으로 이동합니다.");
        });
    }

    private void OnEnable()
    {
        LoadCharacterSelection();
        RefreshCharacterSlots();
    }

    private void OnDisable()
    {
        SaveCharacterSelection();
    }

    public void RefreshCharacterSlots()
    {
        foreach (var slot in filterSlots)
        {
            Destroy(slot.gameObject);
        }
        filterSlots.Clear();

        foreach (var slot in characterSlots)
        {
            Destroy(slot.gameObject);
        }
        characterSlots.Clear();
        addedCharacters.Clear();
        activeChoicePanelSlots.Clear();

        // 새로운 슬롯 생성
        CreateCharacterSlots();
        CreateFilteringSlots();
        LoadCharacterSelection();
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
        
        
        var obtainedGachaIds = GameManager.instance.ResourceManager.ObtainedGachaIDs;
        Logger.Log($"Total obtained Gacha IDs: {obtainedGachaIds.Count}");
        foreach (var characterId in obtainedGachaIds)
        {
            var characterData = playerCharacterTable.Get(characterId);
            if (characterData != null)
            {
                var characterSlot = Instantiate(characterSlotPrefab, filterringSlotParent);
                characterSlot.SetCharacterSlot(characterData);
                characterSlot.OnSlotClick = HandleCharacterSlotClick;
                filterSlots.Add(characterSlot);
                Logger.Log($"Created slot for Character ID: {characterId}");
            }
            else
            {
                Logger.Log($"Character data not found for ID: {characterId}");
            }
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
            
            if (addedCharacters.Count >= 8)
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

        UpdateCharacterIds();
    }

    private void UpdateCharacterIds()
    {
        // Ensure the array is large enough
        if (Variables.LoadTable.characterIds.Length < characterSlots.Count)
        {
            Array.Resize(ref Variables.LoadTable.characterIds, characterSlots.Count);
        }

        for (var i = 0; i < characterSlots.Count; i++)
        {
            if (characterSlots[i].characterData != null)
            {
                Variables.LoadTable.characterIds[i] = characterSlots[i].characterData.Id;
            }
            else
            {
                Variables.LoadTable.characterIds[i] = 0; // 빈 슬롯 처리
            }
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

    private void SaveCharacterSelection()
    {
        for (int i = 0; i < Variables.LoadTable.characterIds.Length; i++)
        {
            PlayerPrefs.SetInt($"CharacterId_{i}", Variables.LoadTable.characterIds[i]);
        }
        PlayerPrefs.Save();
    }

    private void LoadCharacterSelection()
    {
        for (int i = 0; i < Variables.LoadTable.characterIds.Length; i++)
        {
            Variables.LoadTable.characterIds[i] = PlayerPrefs.GetInt($"CharacterId_{i}", 0);
        }

        // 불러온 캐릭터 ID를 슬롯에 설정
        for (int i = 0; i < Variables.LoadTable.characterIds.Length; i++)
        {
            int characterId = Variables.LoadTable.characterIds[i];
            if (characterId != 0)
            {
                var characterData = playerCharacterTable.Get(characterId);
                var slot = characterSlots[i];
                slot.SetCharacterSlot(characterData);
                slot.ChoicePanel.SetActive(false);
                addedCharacters.Add(characterId);
                var originalSlot = filterSlots.FirstOrDefault(s => s.characterData.Id == characterId);
                if (originalSlot != null)
                {
                    originalSlot.ChoiceButton.interactable = false;
                    activeChoicePanelSlots.Add(originalSlot);
                }
            }
        }

        UpdateChoicePanels();
        SortCharacterSlots();
    }
}
