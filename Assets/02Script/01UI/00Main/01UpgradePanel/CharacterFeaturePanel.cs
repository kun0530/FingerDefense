using UnityEngine;
using UnityEngine.UI;

public class CharacterFeaturePanel : MonoBehaviour
{
    private AssetListTable assetListTable;
    private UpgradeTable upgradeTable;
    private StringTable stringTable;

    public Button[] characterArrangementButtons;
    public Button[] characterHpUpgradeButtons;
    public Button[] characterEnhancedGradeButtons;
    
    private void Awake()
    {
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        upgradeTable ??= DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
    }
    private void Start()
    {
        SetupCharacterFeatureButtons();
    }

    private void SetupCharacterFeatureButtons()
    {
        foreach (var upgradeData in upgradeTable.upgradeTable.Values)
        {
            if (upgradeData.Type == 3)
            {
                string assetName = assetListTable.Get(upgradeData.AssetNo);
                Sprite sprite = Resources.Load<Sprite>($"Prefab/06GameIcon/{assetName}"); 

                if (sprite == null)
                {
                    Debug.LogWarning($"AssetNo {upgradeData.AssetNo}에 해당하는 이미지를 찾을 수 없습니다.");
                    continue;
                }

                switch (upgradeData.UpStatType)
                {
                    case 3:
                        if (characterArrangementButtons.Length > 0)
                        {
                            characterArrangementButtons[0].image.sprite = sprite;
                        }
                        break;
                    case 4:
                        if (characterHpUpgradeButtons.Length > 0)
                        {
                            characterHpUpgradeButtons[0].image.sprite = sprite;
                        }
                        break;
                    case 5:
                        if (characterEnhancedGradeButtons.Length > 0)
                        {
                            characterEnhancedGradeButtons[0].image.sprite = sprite;
                        }
                        break;
                    default:
                        Debug.LogWarning($"알 수 없는 UpStatType: {upgradeData.UpStatType}");
                        break;
                }
            }
        }
    }
    
}
