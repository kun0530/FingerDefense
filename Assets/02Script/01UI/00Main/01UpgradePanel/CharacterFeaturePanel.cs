using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CharacterFeaturePanel : MonoBehaviour
{
    private AssetListTable assetListTable;
    private UpgradeTable upgradeTable;
    private StringTable stringTable;

    public Button[] characterArrangementButtons; // UpStatType = 3
    public Button[] characterHpUpgradeButtons; // UpStatType = 4
    public Button[] characterEnhancedGradeButtons; // UpStatType = 5

    public TextMeshProUGUI characterArrangementLevelText;
    public TextMeshProUGUI characterHpLevelText;
    public TextMeshProUGUI characterEnhancedGradeLevelText;
    
    private void Awake()
    {
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        upgradeTable ??= DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
    }

    private void Start()
    {
        SetupCharacterFeatureButtons();
        characterArrangementLevelText.text = stringTable.Get(99972.ToString());
        characterHpLevelText.text = stringTable.Get(99982.ToString());
        characterEnhancedGradeLevelText.text = stringTable.Get(99992.ToString());
    }

    private void SetupCharacterFeatureButtons()
    {
        foreach (var upgradeData in upgradeTable.upgradeTable.Values)
        {
            if (upgradeData.Type == 3)
            {
                string assetName = assetListTable.Get(upgradeData.AssetNo);
                LoadSpriteAsync(assetName, upgradeData);
            }
        }
    }

    private void LoadSpriteAsync(string assetName, UpgradeData upgradeData)
    {
        Addressables.LoadAssetAsync<Sprite>($"Prefab/10UpgradeUI/{assetName}").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite sprite = handle.Result;

                switch (upgradeData.UpStatType)
                {
                    case 3:
                        AssignUpgradeDataToButton(characterArrangementButtons, upgradeData, sprite);
                        break;
                    case 4:
                        AssignUpgradeDataToButton(characterHpUpgradeButtons, upgradeData, sprite);
                        break;
                    case 5:
                        AssignUpgradeDataToButton(characterEnhancedGradeButtons, upgradeData, sprite);
                        break;
                    default:
                        Debug.LogWarning($"알 수 없는 UpStatType: {upgradeData.UpStatType}");
                        break;
                }
            }
            else
            {
                Debug.LogWarning($"AssetNo {upgradeData.AssetNo}에 해당하는 이미지를 Addressables에서 찾을 수 없습니다.");
            }
        };
    }

    private void AssignUpgradeDataToButton(Button[] buttons, UpgradeData upgradeData, Sprite sprite)
    {
        // 정확한 레벨에 맞는 버튼에만 UpgradeData를 할당합니다.
        for (int i = 0; i < buttons.Length; i++)
        {
            if (upgradeData.Level == i + 1) // 버튼 레벨과 UpgradeData의 레벨이 일치하는지 확인
            {
                var button = buttons[i];
                button.image.sprite = sprite;

                button.onClick.RemoveAllListeners(); // 중복 이벤트 방지를 위해 기존 리스너 제거
                button.onClick.AddListener(() =>
                {
                    int currentLevel = GameManager.instance.GameData.PlayerUpgradeLevel
                        .Find(x => x.playerUpgrade == upgradeData.UpStatType).level;

                    if (upgradeData.Level == currentLevel + 1)
                    {
                        TryUpgradeFeature(upgradeData);
                    }
                    else if (upgradeData.Level <= currentLevel)
                    {
                        ModalWindow.Create(window =>
                        {
                            window.SetHeader("이미 업그레이드 완료")
                                .SetBody("이 업그레이드는 이미 완료되었습니다.")
                                .AddButton("확인", () => { })
                                .Show();
                        });
                    }
                    else
                    {
                        ModalWindow.Create(window =>
                        {
                            window.SetHeader("업그레이드 필요")
                                .SetBody("이 업그레이드를 진행하려면 이전 업그레이드를 먼저 완료해 주세요.")
                                .AddButton("확인", () => { })
                                .Show();
                        });

                    }
                });
            }
        }
    }

    private UpgradeData FindUpgradeDataByLevel(int upStatType, int targetLevel)
    {
        foreach (var upgradeData in upgradeTable.upgradeTable.Values)
        {
            if (upgradeData.UpStatType == upStatType && upgradeData.Level == targetLevel)
            {
                return upgradeData;
            }
        }
        return null;
    }

    private void TryUpgradeFeature(UpgradeData upgradeData)
    {
        int playerGold = GameManager.instance.GameData.Gold;
        int stageClearNum = GameManager.instance.GameData.stageClearNum;

        // 현재 버튼에 해당하는 업그레이드 가격과 스테이지 조건을 검사
        if (playerGold >= upgradeData.UpgradePrice && stageClearNum >= upgradeData.NeedClearStage)
        {
            ModalWindow.Create(window =>
            {
                window.SetHeader("구매 확인")
                    .SetBody($"{upgradeData.UpgradePrice} 골드를 사용해서 업그레이드를 진행하시겠습니까?")
                    .AddButton("확인", () =>
                    {
                        GameManager.instance.GameData.Gold -= upgradeData.UpgradePrice;
                        ApplyUpgrade(upgradeData);
                    })
                    .AddButton("취소", () => { })
                    .Show();
            });
        }
        else if (playerGold < upgradeData.UpgradePrice)
        {
            ModalWindow.Create(window =>
            {
                window.SetHeader("구매 실패")
                    .SetBody("골드가 부족합니다.")
                    .AddButton("확인", () => { })
                    .Show();
            });
        }
        else if (stageClearNum < upgradeData.NeedClearStage)
        {
            ModalWindow.Create(window =>
            {
                window.SetHeader("스테이지 클리어 필요")
                    .SetBody($"이 업그레이드를 구매하려면 스테이지 {upgradeData.NeedClearStage}를 클리어해야 합니다.")
                    .AddButton("확인", () => { })
                    .Show();
            });
        }
    }

    private void ApplyUpgrade(UpgradeData upgradeData)
    {
        switch (upgradeData.UpStatType)
        {
            case 3: // CHARACTER_ARRANGEMENT
                GameManager.instance.GameData.UpdatePlayerUpgradeLevel((int)GameData.PlayerUpgrade.CHARACTER_ARRANGEMENT, upgradeData.Level);
                break;
            case 4: // PLAYER_HEALTH
                GameManager.instance.GameData.UpdatePlayerUpgradeLevel((int)GameData.PlayerUpgrade.PLAYER_HEALTH, upgradeData.Level);
                break;
            case 5: // INCREASE_DRAG
                GameManager.instance.GameData.UpdatePlayerUpgradeLevel((int)GameData.PlayerUpgrade.INCREASE_DRAG, upgradeData.Level);
                break;
            default:
                Debug.LogWarning($"알 수 없는 UpStatType: {upgradeData.UpStatType}");
                break;
        }

        // 업그레이드 후 데이터 저장
        DataManager.SaveFile(GameManager.instance.GameData);

        // UI 갱신 로직 추가 (예: 버튼 비활성화 등)
        UpdateUIAfterUpgrade(upgradeData);
    }

    private void UpdateUIAfterUpgrade(UpgradeData upgradeData)
    {
        // 이 메서드에서 UI를 갱신하는 로직을 추가합니다.
    }
}
