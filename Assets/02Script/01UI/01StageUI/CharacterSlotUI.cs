using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
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

    public Image LockImage;
    public delegate void SlotClickDelegate(CharacterSlotUI slot);
    public SlotClickDelegate OnSlotClick;
    public SlotClickDelegate OnLongPress;
    public Action OnLongPressRelease;

    public PlayerCharacterData characterData { get; private set; }

    private Dictionary<int, int> skillIndexMapping = new Dictionary<int, int>();
    private AssetListTable assetListTable;
    public TextMeshProUGUI upgradeLevelText;
    
    private bool isPressed = false;
    public bool isLongPress = false;
    
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
        gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (isLongPress)
        {
            // 롱터치 후 손을 뗐을 때 상태창을 비활성화
            OnLongPressRelease?.Invoke();
        }
        else
        {
            // 일반 터치 시 캐릭터 편성 처리
            OnSlotClick?.Invoke(this);
        }


        isLongPress = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        isLongPress = false;

        PressRoutine().Forget();
    }

    private async UniTaskVoid PressRoutine()
    {
        await UniTask.Delay(500); // 500ms 이상 눌렀을 경우 롱터치로 판단
        if (isPressed)
        {
            isLongPress = true;
            OnLongPress?.Invoke(this); // 롱터치 시 설명창만 활성화
        }
    }

}
