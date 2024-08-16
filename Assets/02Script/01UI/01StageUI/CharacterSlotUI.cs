using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class CharacterSlotUI : MonoBehaviour
{
    public Sprite gradeImage; // gradeParent의 자식으로 추가할 이미지
    public Sprite[] skillImages; // Priority 값을 기준으로 추가할 이미지 배열
    public Sprite[] elementImages; // Element 값을 기준으로 추가할 이미지 배열

    public RectTransform elementParent;
    public RectTransform skillParent;
    public RectTransform gradeParent;
    public RectTransform classParent;
    public TextMeshProUGUI upgradeText;

    public GameObject ChoicePanel;
    public Button ChoiceButton;

    public Image LockImage;
    public delegate void SlotClickDelegate(CharacterSlotUI slot);
    public SlotClickDelegate OnSlotClick;

    public PlayerCharacterData characterData { get; private set; }

    private Dictionary<int, int> skillIndexMapping = new Dictionary<int, int>();
    private AssetListTable assetListTable;
    public TextMeshProUGUI upgradeLevelText;
    
    private void Awake()
    {
        MapSkillsToIndices();
    }
    public void SetLocked(bool isLocked)
    {
        if (LockImage != null)
        {
            LockImage.gameObject.SetActive(isLocked); 
        }
        if (ChoiceButton != null)
        {
            ChoiceButton.interactable = !isLocked;  // 버튼의 상호작용 가능 여부 설정
        }
    }
    
    private void OnEnable()
    {
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);  
    }

    private void MapSkillsToIndices()
    {
        int[] skillValues = { 1000, 1001, 1002 }; // Example skill values
        for (var i = 0; i < skillValues.Length; i++)
        {
            skillIndexMapping[skillValues[i]] = i;
        }
    }

    private int GetSkillIndex(int skillValue)
    {
        return skillIndexMapping.GetValueOrDefault(skillValue, -1);
    }

    public void SetCharacterSlot(PlayerCharacterData characterData)
    {
        this.characterData = characterData;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 기존 Grade 이미지를 제거
        foreach (Transform child in gradeParent)
        {
            Destroy(child.gameObject);
        }

        var assetName = assetListTable.Get(characterData.AssetNo);
        if (!string.IsNullOrEmpty(assetName))
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefab/00CharacterUI/{assetName}");
            if (prefab != null)
            {
                var spineInstance = Instantiate(prefab, classParent);
                spineInstance.transform.localPosition = Vector3.zero; 
            }
        }

        // 새로운 Grade 이미지 추가
        for (var i = 0; i <= characterData.Grade; i++)
        {
            var gradeInstance = new GameObject("GradeImage").AddComponent<Image>();
            gradeInstance.sprite = gradeImage;
            gradeInstance.transform.SetParent(gradeParent, false);
        }

        var elementImage = elementParent.GetComponent<Image>();
        if (elementImage != null && characterData.Element >= 0 && characterData.Element < elementImages.Length)
        {
            elementImage.sprite = elementImages[characterData.Element];
            elementImage.gameObject.SetActive(true);
        }

        var skillImage = skillParent.GetComponent<Image>();
        if (skillImage != null)
        {
            int skillIndex = GetSkillIndex(characterData.SkillIcon);
            if (skillIndex >= 0 && skillIndex < skillImages.Length)
            {
                skillImage.sprite = skillImages[skillIndex];
                skillImage.gameObject.SetActive(true);
            }
            else
            {
                skillImage.gameObject.SetActive(false);
            }
        }

        upgradeLevelText.text = $"+ {characterData.Plus}";
        ChoicePanel.transform.SetAsLastSibling();

        ChoiceButton.onClick.AddListener(OnClick);
    }


    public void ClearSlot()
    {
        characterData = null;

        foreach (Transform child in gradeParent)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in classParent)
        {
            child.gameObject.SetActive(false);
        }

        var elementImage = elementParent.GetComponent<Image>();
        if (elementImage != null)
        {
            elementImage.sprite = null;
            elementImage.gameObject.SetActive(false);
        }

        var skillImage = skillParent.GetComponent<Image>();
        if (skillImage != null)
        {
            skillImage.sprite = null;
            skillImage.gameObject.SetActive(false);
        }
        upgradeLevelText.text = "";
        ChoicePanel.SetActive(false);
        ChoiceButton.interactable = true; 
        ChoiceButton.onClick.RemoveAllListeners(); 
        gameObject.SetActive(true);
    }


    
    private void OnClick()
    {
        OnSlotClick?.Invoke(this);
    }
}
