using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;


public class StageSlot : MonoBehaviour
{
    public TextMeshProUGUI stageNameText;
    public TextMeshProUGUI monsterText;
    public TextMeshProUGUI rewardText;
    public RectTransform monsterSlotParent;
    public RectTransform rewardSlotParent;

    public GameObject monsterSlotPrefab; 
    public GameObject rewardSlotPrefab;
    public Button DeckButton;

    private int StageId;
    private AssetListTable assetListTable;
    private GameObject stageMask;
    private GameManager gameManager;
    
    [SerializeField]private GameObject deckUI;
    public TutorialController stagePanelController;
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
    }
    public void SetStageMask(GameObject stageMask)
    {
        this.stageMask = stageMask;
    }
    public void GameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public void SetStagePanelController(TutorialController stagePanelController)
    {
        this.stagePanelController = stagePanelController;
    }
    
    public void Configure(StageData stageData)
    {
        //해당 슬롯에 스테이지 이름 설정 =>stageData.StageNameId을 토대로 StringTable에서 찾아서 가져오기 
        stageNameText.text = DataTableManager.Get<StringTable>(DataTableIds.String).Get(stageData.StageNameId.ToString());
        monsterText.text = DataTableManager.Get<StringTable>(DataTableIds.String).Get(98921.ToString());
        rewardText.text = DataTableManager.Get<StringTable>(DataTableIds.String).Get(98931.ToString());
        
        StageId = stageData.StageId;
        if (stageData.Monster1Id != 0) AddMonsterSlot(stageData.Monster1Id);
        if (stageData.Monster2Id != 0) AddMonsterSlot(stageData.Monster2Id);
        if (stageData.Monster3Id != 0) AddMonsterSlot(stageData.Monster3Id);
        if (stageData.Monster4Id != 0) AddMonsterSlot(stageData.Monster4Id);

        if (stageData.Reward1Id != 0 && stageData.Reward1Value != 0) AddRewardSlot(stageData.Reward1Id, stageData.Reward1Value);
        //if (stageData.Reward2Id != 0 && stageData.Reward2Value != 0) AddRewardSlot(stageData.Reward2Id, stageData.Reward2Value);
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
       
        // TO-DO: Addressable로 변경 예정 
        // if (assetListTable == null)
        // {
        //     Logger.LogError("AssetListTable is not set.");
        //     return;
        // }
        //
        // // AssetListTable을 사용하여 프리팹 이름 가져오기
        // string prefabName = assetListTable.Get(monsterId);
        // if (!string.IsNullOrEmpty(prefabName))
        // {
        //     // Addressables를 사용하여 프리팹 로드
        //     Addressables.LoadAssetAsync<GameObject>($"Prefab/01MonsterUI/{prefabName}").Completed += handle =>
        //     {
        //         if (handle.Status == AsyncOperationStatus.Succeeded)
        //         {
        //             GameObject monsterPrefab = handle.Result;
        //             var monsterSlot = Instantiate(monsterSlotPrefab, monsterSlotParent);
        //             var monsterInstance = Instantiate(monsterPrefab, monsterSlot.transform); // monsterSlot의 자식으로 추가
        //             monsterInstance.transform.localPosition = new Vector3(0, 3, 0); // 필요한 경우 위치 조정
        //             monsterInstance.transform.localScale = Vector3.one; // 필요한 경우 스케일 조정
        //
        //             var monsterText = monsterSlot.GetComponentInChildren<TextMeshProUGUI>();
        //             //monsterText.text = monsterId.ToString();
        //         }
        //         else
        //         {
        //             Logger.LogWarning($"Prefab not found for {prefabName}");
        //         }
        //     };
        // }
        // else
        // {
        //     Logger.LogWarning($"Prefab name not found for Monster ID: {monsterId}");
        // }
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
        
        //AssetListTable 업데이트되면 적용 예정 
        // if (assetListTable == null)
        // {
        //     Logger.LogError("AssetListTable is not set.");
        //     return;
        // }
        //
        // // AssetListTable을 사용하여 프리팹 이름 가져오기
        // string prefabName = assetListTable.Get(rewardId);
        // if (!string.IsNullOrEmpty(prefabName))
        // {
        //     // Resources.Load를 사용하여 프리팹 로드 ,To-Do: Addressable로 변경 예정
        //     GameObject rewardPrefab = Resources.Load<GameObject>($"Prefab/04RewardImage/{prefabName}");
        //     if (rewardPrefab != null)
        //     {
        //         var rewardSlot = Instantiate(rewardSlotPrefab, rewardSlotParent);
        //         var rewardInstance = Instantiate(rewardPrefab, rewardSlot.transform); 
        //         rewardInstance.transform.localPosition = new Vector3(0,3,0); // 필요한 경우 위치 조정
        //         rewardInstance.transform.localScale = Vector3.one; // 필요한 경우 스케일 조정
        //
        //         var rewardText = rewardSlot.GetComponentInChildren<TextMeshProUGUI>();
        //         rewardText.text = rewardValue.ToString();
        //     }
        //     else
        //     {
        //         Logger.LogWarning($"Prefab not found for {prefabName}");
        //     }
        // }
        // else
        // {
        //     Logger.LogWarning($"Prefab name not found for Reward ID: {rewardId}");
        // }
        
    }
    
    public void OnClick()
    {
        if (stageMask.gameObject.activeSelf)
        {
            stagePanelController.enabled = true;   
            Variables.LoadTable.StageId = StageId;
            SceneManager.LoadScene(2);
        }
        else
        {
            deckUI.SetActive(true);
            Variables.LoadTable.StageId = StageId;
            deckUI.transform.SetAsLastSibling();
            Logger.Log($"스테이지 {StageId} 선택");    
        }
    }
}
