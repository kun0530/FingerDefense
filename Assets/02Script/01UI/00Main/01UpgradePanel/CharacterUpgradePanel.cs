using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUpgradePanel : MonoBehaviour
{
    private AssetListTable assetListTable;
    private UpgradeTable upgradeTable;
    private StringTable stringTable;
    private PlayerCharacterTable playerCharacterTable;
    
    public CharacterUpgradeSlotUI characterUpgradeSlot;
    public RectTransform characterUpgradeSlotContent; //캐릭터 업그레이드를 하기 전의 현재 있는 캐릭터를 담는 부모
    
    public RectTransform characterUpgradeSlotParent; //캐릭터 업그레이드를 하기 전의 슬롯
    public RectTransform characterUpgradeResultSlotContent; //캐릭터 업그레이드를 할때 그 결과를 보여줄 슬롯
    
    public TextMeshProUGUI characterUpgradeResultText; 
    public TextMeshProUGUI characterUpgradeGoldText;
    
    private List<int> obtainedGachaIds;
    
    public Button upgradeButton;

    private UpgradeData selectedUpgradeData;
    public static event Action<int, int> OnCharacterUpgraded;
    public void Awake()
    {
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        upgradeTable ??= DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
        playerCharacterTable ??= DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
    }
    
    public void Start()
    {
        RefreshPanel();       
    }
    public void RefreshPanel()
    {
        obtainedGachaIds = GameManager.instance.GameData.ObtainedGachaIDs;
        LoadUpgradableCharacters();
    }
    private void LoadUpgradableCharacters()
    {
        // 기존 슬롯 초기화
        ClearSlot(characterUpgradeSlotContent);

        foreach (var upgradeData in upgradeTable.upgradeTable.Values)
        {
            if (upgradeData.Type == 2 && obtainedGachaIds.Contains(upgradeData.NeedCharId))
            {
                var characterData = playerCharacterTable.Get(upgradeData.NeedCharId);
                if (characterData != null)
                {
                    var characterSlot = Instantiate(characterUpgradeSlot, characterUpgradeSlotContent);
                    characterSlot.SetCharacterSlot(characterData);
                    characterSlot.OnSlotClick = (slot) => DisplayUpgradeOptions(upgradeData, characterData);
                }
            }
        }
    }

    private void DisplayUpgradeOptions(UpgradeData upgradeData, PlayerCharacterData characterData)
    {
        selectedUpgradeData = upgradeData; // 선택된 업그레이드 데이터를 저장

        // 선택된 캐릭터 정보를 characterUpgradeSlotParent에 표시
        ClearSlot(characterUpgradeSlotParent); // 기존 슬롯 비우기
        var selectedSlot = Instantiate(characterUpgradeSlot, characterUpgradeSlotParent);
        selectedSlot.SetCharacterSlot(characterData);

        // UpgradeResultId를 기반으로 업그레이드 결과 표시
        ClearSlot(characterUpgradeResultSlotContent); // 기존 결과 슬롯 비우기
        var resultCharacterData = playerCharacterTable.Get(upgradeData.UpgradeResultId);
        if (resultCharacterData != null)
        {
            var resultSlot = Instantiate(characterUpgradeSlot, characterUpgradeResultSlotContent);
            resultSlot.SetCharacterSlot(resultCharacterData);
            characterUpgradeResultText.text = stringTable.Get(upgradeData.UpgradeInfoId.ToString()); // 업그레이드 설명 표시
        }

        // 업그레이드 가격 표시
        characterUpgradeGoldText.text = upgradeData.UpgradePrice.ToString();

        // 업그레이드 버튼 활성화 또는 비활성화
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(TryUpgradeCharacter);
        upgradeButton.interactable = GameManager.instance.GameData.stageClearNum >= upgradeData.NeedClearStage;
    }

    private void TryUpgradeCharacter()
    {
        if (selectedUpgradeData == null) return;

        int playerGold = GameManager.instance.GameData.Gold;

        // 현재 버튼에 해당하는 업그레이드 가격과 스테이지 조건을 검사
        if (playerGold >= selectedUpgradeData.UpgradePrice)
        {
            ModalWindow.Create()
                .SetHeader("구매 확인")
                .SetBody($"{selectedUpgradeData.UpgradePrice} 골드를 사용해서 업그레이드를 진행하시겠습니까?")
                .AddButton("확인", () =>
                {
                    GameManager.instance.GameData.Gold -= selectedUpgradeData.UpgradePrice;
                    ApplyUpgrade(selectedUpgradeData);
                })
                .AddButton("취소", () => { })
                .Show();
        }
        else
        {
            ModalWindow.Create()
                .SetHeader("구매 실패")
                .SetBody("골드가 부족합니다.")
                .AddButton("확인", () => { })
                .Show();
        }
    }

    private void ApplyUpgrade(UpgradeData upgradeData)
    {
        // 업그레이드 적용 로직 (예: 캐릭터의 등급을 업그레이드)
        var existingIndex = GameManager.instance.GameData.ObtainedGachaIDs.IndexOf(upgradeData.NeedCharId);
        if (existingIndex >= 0)
        {
            GameManager.instance.GameData.ObtainedGachaIDs[existingIndex] = upgradeData.UpgradeResultId;
        }
        // 업그레이드 이벤트 호출
        OnCharacterUpgraded?.Invoke(upgradeData.NeedCharId, upgradeData.UpgradeResultId);
        // 데이터 저장
        DataManager.SaveFile(GameManager.instance.GameData);

        // UI 갱신
        ClearSlot(characterUpgradeSlotParent);
        ClearSlot(characterUpgradeResultSlotContent);
        LoadUpgradableCharacters();  // 업그레이드 가능한 캐릭터 목록 다시 로드
    }


    private void ClearSlot(RectTransform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
