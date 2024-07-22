using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class StageSlot : MonoBehaviour
{
    public TextMeshProUGUI stageNameText;

    public RectTransform monsterSlotParent;
    public RectTransform rewardSlotParent;

    public GameObject monsterSlotPrefab; 
    public GameObject rewardSlotPrefab;
    public Button DeckButton;

    private int StageId;
    private AssetListTable assetListTable;
    
    [SerializeField]private GameObject deckUI;

    public void Start()
    { 
        DeckButton.onClick.AddListener(OnClick);
    }
    public void SetDeckUI(GameObject deckUI)
    {
        this.deckUI = deckUI;
    }
    public void SetAssetListTable(AssetListTable assetListTable)
    {
        this.assetListTable = assetListTable;
        Logger.Log("SetAssetListTable");
    }
    public void Configure(StageData stageData)
    {
        stageNameText.text = stageData.StageNameId.ToString();
        StageId = stageData.StageId;
        if (stageData.Monster1Id != 0) AddMonsterSlot(stageData.Monster1Id);
        if (stageData.Monster2Id != 0) AddMonsterSlot(stageData.Monster2Id);
        if (stageData.Monster3Id != 0) AddMonsterSlot(stageData.Monster3Id);

        if (stageData.Reward1Id != 0 && stageData.Reward1Value != 0) AddRewardSlot(stageData.Reward1Id, stageData.Reward1Value);
        if (stageData.Reward2Id != 0 && stageData.Reward2Value != 0) AddRewardSlot(stageData.Reward2Id, stageData.Reward2Value);
    }

    private void AddMonsterSlot(int monsterId)
    {
        if (assetListTable == null)
        {
            Logger.LogError("AssetListTable is not set.");
            return;
        }

        // AssetListTable을 사용하여 프리팹 이름 가져오기
        string prefabName = assetListTable.Get(monsterId);
        if (!string.IsNullOrEmpty(prefabName))
        {
            // Resources.Load를 사용하여 프리팹 로드 ,To-Do: Addressable로 변경 예정
            GameObject monsterPrefab = Resources.Load<GameObject>($"Prefab/01MonsterUI/{prefabName}");
            if (monsterPrefab != null)
            {
                var monsterSlot = Instantiate(monsterSlotPrefab, monsterSlotParent);
                var monsterInstance = Instantiate(monsterPrefab, monsterSlot.transform); // monsterSlot의 자식으로 추가
                monsterInstance.transform.localPosition = new Vector3(0,3,0); // 필요한 경우 위치 조정
                monsterInstance.transform.localScale = Vector3.one; // 필요한 경우 스케일 조정

                var monsterText = monsterSlot.GetComponentInChildren<TextMeshProUGUI>();
                //monsterText.text = monsterId.ToString();
            }
            else
            {
                Logger.LogWarning($"Prefab not found for {prefabName}");
            }
        }
        else
        {
            Logger.LogWarning($"Prefab name not found for Monster ID: {monsterId}");
        }
       
    }

    private void AddRewardSlot(int rewardId, int rewardValue)
    {
        GameObject rewardSlot = Instantiate(rewardSlotPrefab, rewardSlotParent);
        Image rewardImage = rewardSlot.GetComponentInChildren<Image>();
        TextMeshProUGUI rewardText = rewardSlot.GetComponentInChildren<TextMeshProUGUI>();

        // To-Do 데이터 테이블로 불러올 수 있도록 수정 예정
        Sprite rewardSprite = Resources.Load<Sprite>($"Rewards/{rewardId}") ? Resources.Load<Sprite>($"Rewards/{rewardId}") : Resources.Load<Sprite>("PlaceholderImage");
        if (rewardSprite != null)
        {
            rewardImage.sprite = rewardSprite;
        }
        else
        {
            Logger.LogWarning($"Reward image not found for ID: {rewardId}");
        }

        rewardText.text = $"{rewardValue}";
    }
    
    public void OnClick()
    {
        deckUI.SetActive(true);
        Variables.LoadTable.stageId = StageId;
        Logger.Log($"스테이지 {StageId} 선택");
    }
}
