using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour
{
    public Sprite gradeImage; // gradeParent의 자식으로 추가할 이미지
    public Sprite[] skillImages; // Priority 값을 기준으로 추가할 이미지 배열
    public Sprite[] elementImages; // Element 값을 기준으로 추가할 이미지 배열

    public RectTransform elementParent;
    public RectTransform skillParent;
    public RectTransform gradeParent;
    public RectTransform classParent;

    public GameObject[] spinePrefabs; // AssetNo에 따라 스폰할 스파인 프리팹 배열
    public GameObject ChoicePanel;
    public Button ChoiceButton;

    public delegate void SlotClickDelegate(CharacterSlotUI slot);
    public SlotClickDelegate OnSlotClick;

    public PlayerCharacterData characterData { get; private set; }

    public void SetCharacterSlot(PlayerCharacterData characterData)
    {
        this.characterData = characterData;
        
        if (characterData.AssetNo >= 0 && characterData.AssetNo < spinePrefabs.Length)
        {
            var spineInstance = Instantiate(spinePrefabs[characterData.AssetNo], classParent);
            spineInstance.transform.localPosition = Vector3.zero; // 필요한 경우 위치 조정
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
            elementImage.gameObject.SetActive(true); // 활성화
        }
        
        var skillImage = skillParent.GetComponent<Image>();
        if (skillImage != null && characterData.Priority >= 0 && characterData.Priority < skillImages.Length)
        {
            skillImage.sprite = skillImages[characterData.Priority];
            skillImage.gameObject.SetActive(true); // 활성화
        }
        
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
