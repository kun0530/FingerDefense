using UnityEngine;
using UnityEngine.UI;

public class MonsterGimmickPanel : MonoBehaviour
{
    private UpgradeTable upgradeTable;
    private StringTable stringTable;
    private AssetListTable assetListTable;

    public Button[] GimmickRangeUpgradeButtons; //UpStatType = 0
    public Button[] GimmickDamageUpgradeButtons; //UpStatType = 1
    public Button[] GimmickDurationUpgradeButtons; //UpStatType = 2
    
    private void Awake()
    {
        upgradeTable ??= DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
    }
    
    private void Start()
    {
        SetupGimmickUpgradeButtons();
        for(var i=0; i<GimmickRangeUpgradeButtons.Length; i++)
        {
            var index = i;
            GimmickRangeUpgradeButtons[index].onClick.AddListener(() => OnClickGimmickRangeUpgradeButton(index));
        }
        for(var i=0; i<GimmickDamageUpgradeButtons.Length; i++)
        {
            var index = i;
            GimmickDamageUpgradeButtons[index].onClick.AddListener(() => OnClickGimmickDamageUpgradeButton(index));
        }
        for(var i=0; i<GimmickDurationUpgradeButtons.Length; i++)
        {
            var index = i;
            GimmickDurationUpgradeButtons[index].onClick.AddListener(() => OnClickGimmickDurationGradeButton(index));
        }
    }
    private void OnClickGimmickRangeUpgradeButton(int index)
    {
        
    }
    private void OnClickGimmickDamageUpgradeButton(int index)
    {
        
    }
    private void OnClickGimmickDurationGradeButton(int index)
    {
        
    }

    
   
    
    private void SetupGimmickUpgradeButtons()
    {
        foreach (var upgradeData in upgradeTable.upgradeTable.Values)
        {
            if (upgradeData.Type == 1)
            {
                string assetName = assetListTable.Get(upgradeData.AssetNo);
                Sprite sprite = Resources.Load<Sprite>($"Prefab/06ShopIcon/{assetName}"); // 경로는 프로젝트 구조에 맞게 수정

                if (sprite == null)
                {
                    Debug.LogWarning($"AssetNo {upgradeData.AssetNo}에 해당하는 이미지를 찾을 수 없습니다.");
                    continue;
                }

                switch (upgradeData.UpStatType)
                {
                    case 0:
                        if (GimmickRangeUpgradeButtons.Length > 0)
                        {
                            GimmickRangeUpgradeButtons[0].image.sprite = sprite;
                        }
                        break;
                    case 1:
                        if (GimmickDamageUpgradeButtons.Length > 0)
                        {
                            GimmickDamageUpgradeButtons[0].image.sprite = sprite;
                        }
                        break;
                    case 2:
                        if (GimmickDurationUpgradeButtons.Length > 0)
                        {
                            GimmickDurationUpgradeButtons[0].image.sprite = sprite;
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
