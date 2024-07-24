using System.Collections.Generic;
using UnityEngine;

public class StageSlotCreate : MonoBehaviour
{
    private StageTable stageTable;
    public StageSlot stageSlotPrefab;
    public RectTransform[] slotParents;
    public GameObject deckUI;
    private AssetListTable assetListTable;
    private StringTable stringTable;
    
    //포로토타입용 삭제 예정
    private GameManager gameManager;
    private DeckUITutorialManager tutorialManager;
    
    private void Start()
    {
        stageTable ??= DataTableManager.Get<StageTable>(DataTableIds.Stage);
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
        gameManager = GameObject.FindGameObjectWithTag("Manager")?.GetComponent<GameManager>();
        tutorialManager = GameObject.FindGameObjectWithTag("Tutorial")?.GetComponentInChildren<DeckUITutorialManager>();
        CreateStageSlots();
    }

    private void CreateStageSlots()
    {
        List<StageData> batch = new List<StageData>();
        int parentIndex = 0;

        foreach (var stageData in stageTable.GetAll())
        {
            batch.Add(stageData);

            if (batch.Count == 5)
            {
                CreateBatch(batch, slotParents[parentIndex]);
                batch.Clear();
                parentIndex = (parentIndex + 1) % slotParents.Length;
            }
        }
        
        if (batch.Count > 0)
        {
            CreateBatch(batch, slotParents[parentIndex]);
        }
    }

    private void CreateBatch(List<StageData> batch, RectTransform parent)
    {
        foreach (var stageData in batch)
        {
            StageSlot slot = Instantiate(stageSlotPrefab, parent);
            slot.SetAssetListTable(assetListTable);
            slot.Configure(stageData);
            slot.SetDeckUI(deckUI);
            slot.SetManagers(gameManager, tutorialManager);
        }
    }
}