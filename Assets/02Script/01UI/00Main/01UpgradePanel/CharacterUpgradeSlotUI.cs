using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUpgradeSlotUI : MonoBehaviour
{
    public Sprite gradeImage;
    
    public Sprite[] elementImages;
    
    public RectTransform elementParent;
    public RectTransform gradeParent;
    public RectTransform classParent;
    
    public GameObject ChoicePanel;
    public Button ChoiceButton;
    public Image SkillIcon;
    
    public delegate void SlotClickDelegate(CharacterUpgradeSlotUI slot);
    public SlotClickDelegate OnSlotClick;
    
    public PlayerCharacterData characterData { get; private set; }
    
    private AssetListTable assetListTable;
    private SkillTable skillTable;
    public TextMeshProUGUI upgradeLevelText;

    public TextMeshProUGUI powerText;
    
    private void OnEnable()
    {
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
        
    }
    
    public void SetCharacterSlot(PlayerCharacterData characterData)
    {
        this.characterData = characterData;

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
        upgradeLevelText.text = $"+ {characterData.Plus}";
        
        var skillImage = SkillIcon.GetComponent<Image>();
        var skillId = assetListTable.Get(characterData.SkillIcon);
        if(skillImage != null && !string.IsNullOrEmpty(skillId))
        {
            skillImage.sprite = Resources.Load<Sprite>($"Prefab/09SkillIcon/{skillId}");
            skillImage.gameObject.SetActive(true);
        }

        powerText.text = $"{characterData.Power}";

        ChoicePanel.transform.SetAsLastSibling();
        ChoiceButton.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        OnSlotClick?.Invoke(this);
    }
} 
