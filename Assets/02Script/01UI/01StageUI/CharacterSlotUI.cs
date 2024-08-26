using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class CharacterSlotUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Sprite gradeImage; // gradeParent의 자식으로 추가할 이미지
    public Sprite[] elementImages; // Element 값을 기준으로 추가할 이미지 배열

    public RectTransform elementParent;
    public Image SkillIcon;
    public RectTransform gradeParent;
    public RectTransform classParent;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI powerText;

    public GameObject ChoicePanel;

    public Image LockImage;
    public Image PanelImage;
    public Image PowerImage;

    public delegate void SlotClickDelegate(CharacterSlotUI slot);

    public SlotClickDelegate OnSlotClick;
    public SlotClickDelegate OnLongPress;
    public Action OnLongPressRelease;

    public PlayerCharacterData characterData { get; private set; }

    private Dictionary<int, int> skillIndexMapping = new Dictionary<int, int>();
    private AssetListTable assetListTable;
    private StringTable stringTable;
    public TextMeshProUGUI upgradeLevelText;

    private bool isPressed = false;
    public bool isLongPress = false;

    private GameObject spineInstance;

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
        stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);
    }

    public void SetCharacterSlot(PlayerCharacterData characterData)
    {
        this.characterData = characterData;
        UpdateUI().Forget();
    }

    private async UniTaskVoid UpdateUI()
    {
        // 현재 오브젝트가 파괴될 때 작업을 취소하기 위한 CancellationToken을 가져옵니다.
        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();

        // 기존 Grade 이미지를 제거
        foreach (Transform child in gradeParent)
        {
            Destroy(child.gameObject);
        }

        var assetName = assetListTable.Get(characterData.AssetNo);
        if (!string.IsNullOrEmpty(assetName))
        {
            // Addressables로 프리팹 로드
            var handle = Addressables.LoadAssetAsync<GameObject>($"Prefab/00CharacterUI/{assetName}");
            GameObject prefab = await handle.WithCancellation(cancellationToken); // 외부 CancellationToken 적용

            if (prefab != null)
            {
                // 기존 인스턴스가 존재하면 제거
                if (spineInstance != null)
                {
                    Destroy(spineInstance);
                }

                spineInstance = Instantiate(prefab, classParent);
                spineInstance.transform.localPosition = Vector3.zero;
            }
            else
            {
                Logger.LogWarning($"Prefab not found for {assetName}");
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

        var skillId = assetListTable.Get(characterData.SkillIcon);
        if (!string.IsNullOrEmpty(skillId))
        {
            // Addressables로 스킬 아이콘 로드
            var skillHandle = Addressables.LoadAssetAsync<Sprite>($"Prefab/09SkillIcon/{skillId}");
            Sprite skillSprite = await skillHandle.WithCancellation(cancellationToken); // 외부 CancellationToken 적용

            if (skillSprite != null)
            {
                SkillIcon.sprite = skillSprite;
                SkillIcon.gameObject.SetActive(true);
            }
            else
            {
                Logger.LogWarning($"Skill icon not found for {skillId}");
            }
        }

        if (PowerImage != null)
        {
            PowerImage.gameObject.SetActive(true);
        }

        powerText.text = $"+{characterData.Power}";
        powerText.gameObject.transform.SetAsLastSibling();

        upgradeLevelText.text = $"+{characterData.Plus}";
        ChoicePanel.transform.SetAsLastSibling();
    }


    public void ClearSlot()
    {
        // 캐릭터 데이터 초기화
        characterData = null;

        // 캐릭터 이름, 레벨 등 텍스트 초기화
        if (upgradeLevelText != null)
        {
            upgradeLevelText.text = "";
        }

        if (powerText != null)
        {
            powerText.text = "";
        }

        // 스파인 인스턴스 삭제
        if (spineInstance != null)
        {
            Destroy(spineInstance);
            spineInstance = null;
        }

        if (gradeParent != null)
        {
            foreach (Transform child in gradeParent)
            {
                Destroy(child.gameObject);
            }
        }

        var elementImage = elementParent.GetComponent<Image>();
        if (elementImage != null)
        {
            elementImage.gameObject.SetActive(false);
        }

        var skillImage = GetComponent<Image>();
        if (skillImage != null)
        {
            skillImage.gameObject.SetActive(false);
        }

        if (PowerImage != null)
        {
            PowerImage.gameObject.SetActive(false);
        }


        // 슬롯 재활용 가능 상태로 설정
        ChoicePanel.gameObject.SetActive(false);
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