using System;
using System.Collections.Generic;
using Spine.Unity;
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

    public GameObject ChoicePanel;
    public Button ChoiceButton;

    public delegate void SlotClickDelegate(CharacterSlotUI slot);
    public SlotClickDelegate OnSlotClick;

    public PlayerCharacterData characterData { get; private set; }

    private Dictionary<int, int> skillIndexMapping = new Dictionary<int, int>();
    private AssetListTable assetListTable;
    
    private void Awake()
    {
        MapSkillsToIndices();
        
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
        
        var assetName = assetListTable.Get(characterData.AssetNo);
        if (!string.IsNullOrEmpty(assetName))
        {
            // TO-DO: Addressables.LoadAssetAsync<GameObject>($"Prefabs/{assetName}")를 사용하여 프리팹을 로드하도록 수정해야 합니다.
            //GameObject prefab = Addressables.LoadAssetAsync<GameObject>($"Prefab/{assetName}").WaitForCompletion();
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
            // 필요한 경우 gradeInstance에 추가적인 설정
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
            int skillIndex = GetSkillIndex(characterData.Skill);
            Logger.Log("Character Skill: " + characterData.Skill + ", Mapped Index: " + skillIndex);
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
        ChoicePanel.transform.SetAsLastSibling();
        
        ChoiceButton.onClick.AddListener(OnClick);
    }

    public void ClearSlot()
    {
        characterData = null;
        
        foreach (Transform child in classParent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in gradeParent)
        {
            Destroy(child.gameObject);
        }
        
        var elementImage = elementParent.GetComponent<Image>();
        if (elementImage != null)
        {
            elementImage.gameObject.SetActive(false);
        }
        
        var skillImage = skillParent.GetComponent<Image>();
        if (skillImage != null)
        {
            skillImage.gameObject.SetActive(false);
        }
        
        ChoicePanel.SetActive(false);
        ChoiceButton.interactable = false;
    }

    private void OnClick()
    {
        OnSlotClick?.Invoke(this);
    }
}
