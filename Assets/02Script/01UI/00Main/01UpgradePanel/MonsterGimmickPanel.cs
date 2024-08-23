using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterGimmickPanel : MonoBehaviour
{
    private UpgradeTable upgradeTable;
    private StringTable stringTable;
    private AssetListTable assetListTable;

    public Button[] GimmickRangeUpgradeButtons; // UpStatType = 0
    public Button[] GimmickDamageUpgradeButtons; // UpStatType = 1
    public Button[] GimmickDurationUpgradeButtons; // UpStatType = 2
    
    public TextMeshProUGUI GimmickRangeLevelText;
    public TextMeshProUGUI GimmickDamageLevelText;
    public TextMeshProUGUI GimmickDurationLevelText;
    
    private void Awake()
    {
        upgradeTable ??= DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
    }

    private void Start()
    {
        SetupGimmickUpgradeButtons();
        GimmickRangeLevelText.text = stringTable.Get(99942.ToString());
        GimmickDamageLevelText.text = stringTable.Get(99952.ToString());
        GimmickDurationLevelText.text = stringTable.Get(99962.ToString());
    }

    private void SetupGimmickUpgradeButtons()
    {
        foreach (var upgradeData in upgradeTable.upgradeTable.Values)
        {
            if (upgradeData.Type == 1)
            {
                string assetName = assetListTable.Get(upgradeData.AssetNo);
                Sprite sprite = Resources.Load<Sprite>($"Prefab/10UpgradeUI/{assetName}");

                if (sprite == null)
                {
                    Debug.LogWarning($"AssetNo {upgradeData.AssetNo}에 해당하는 이미지를 찾을 수 없습니다.");
                    continue;
                }

                switch (upgradeData.UpStatType)
                {
                    case 0:
                        SetupButtonGroup(GimmickRangeUpgradeButtons, upgradeData, sprite);
                        break;
                    case 1:
                        SetupButtonGroup(GimmickDamageUpgradeButtons, upgradeData, sprite);
                        break;
                    case 2:
                        SetupButtonGroup(GimmickDurationUpgradeButtons, upgradeData, sprite);
                        break;
                    default:
                        Debug.LogWarning($"알 수 없는 UpStatType: {upgradeData.UpStatType}");
                        break;
                }
            }
        }
    }

    private void SetupButtonGroup(Button[] buttons, UpgradeData upgradeData, Sprite sprite)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            button.image.sprite = sprite;

            // 각 버튼에 대해 해당 레벨에 맞는 UpgradeData를 찾아 설정
            int targetLevel = i + 1;
            UpgradeData targetUpgradeData = FindUpgradeDataByLevel(upgradeData.UpStatType, targetLevel);

            if (targetUpgradeData != null)
            {
                button.onClick.RemoveAllListeners(); // 중복 이벤트 방지를 위해 기존 리스너 제거
                button.onClick.AddListener(() =>
                {
                    int currentLevel = GameManager.instance.GameData.MonsterGimmickLevel
                        .Find(x => x.monsterGimmick == upgradeData.UpStatType).level;

                    if (targetLevel == currentLevel + 1)
                    {
                        TryUpgradeGimmick(targetUpgradeData);
                    }
                    else if (targetLevel <= currentLevel)
                    {
                        ModalWindow.Create()
                            .SetHeader("이미 업그레이드 완료")
                            .SetBody("이 업그레이드는 이미 완료되었습니다.")
                            .AddButton("확인", () => { })
                            .Show();
                    }
                    else
                    {
                        ModalWindow.Create()
                            .SetHeader("업그레이드 필요")
                            .SetBody("이 업그레이드를 진행하려면 이전 업그레이드를 먼저 완료해 주세요.")
                            .AddButton("확인", () => { })
                            .Show();
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

    private void TryUpgradeGimmick(UpgradeData upgradeData)
    {
        int playerGold = GameManager.instance.GameData.Gold;
        int stageClearNum = GameManager.instance.GameData.stageClearNum;

        // 현재 버튼에 해당하는 업그레이드 가격과 스테이지 조건을 검사
        if (playerGold >= upgradeData.UpgradePrice && stageClearNum >= upgradeData.NeedClearStage)
        {
            ModalWindow.Create()
                .SetHeader("구매 확인")
                .SetBody($"{upgradeData.UpgradePrice} 골드를 사용해서 업그레이드를 진행하시겠습니까?")
                .AddButton("확인", () =>
                {
                    GameManager.instance.GameData.Gold -= upgradeData.UpgradePrice;
                    ApplyGimmickUpgrade(upgradeData);
                })
                .AddButton("취소", () => { })
                .Show();
        }
        else if (playerGold < upgradeData.UpgradePrice)
        {
            ModalWindow.Create()
                .SetHeader("구매 실패")
                .SetBody("골드가 부족합니다.")
                .AddButton("확인", () => { })
                .Show();
        }
        else if (stageClearNum < upgradeData.NeedClearStage)
        {
            ModalWindow.Create()
                .SetHeader("스테이지 클리어 필요")
                .SetBody($"이 업그레이드를 구매하려면 스테이지 {upgradeData.NeedClearStage}를 클리어해야 합니다.")
                .AddButton("확인", () => { })
                .Show();
        }
    }

    private void ApplyGimmickUpgrade(UpgradeData upgradeData)
    {
        switch (upgradeData.UpStatType)
        {
            case 0: // ATTACK RANGE
                GameManager.instance.GameData.UpdateMonsterGimmickLevel((int)GameData.MonsterGimmick.ATTACKRANGE, upgradeData.Level);
                break;
            case 1: // ATTACK DAMAGE
                GameManager.instance.GameData.UpdateMonsterGimmickLevel((int)GameData.MonsterGimmick.ATTACKDAMAGE, upgradeData.Level);
                break;
            case 2: // ATTACK DURATION
                GameManager.instance.GameData.UpdateMonsterGimmickLevel((int)GameData.MonsterGimmick.ATTACKDURATION, upgradeData.Level);
                break;
            default:
                Debug.LogWarning($"알 수 없는 UpStatType: {upgradeData.UpStatType}");
                break;
        }

        // 업그레이드 후 데이터 저장
        DataManager.SaveFile(GameManager.instance.GameData);

        // UI 갱신 로직 추가 (예: 버튼 비활성화 등)
        UpdateUIAfterGimmickUpgrade(upgradeData);
    }

    private void UpdateUIAfterGimmickUpgrade(UpgradeData upgradeData)
    {
        // 이 메서드에서 UI를 갱신하는 로직을 추가합니다.
    }
}
