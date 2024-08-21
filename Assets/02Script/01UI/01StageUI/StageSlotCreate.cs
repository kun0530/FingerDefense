using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageSlotCreate : MonoBehaviour
{
    private StageTable stageTable;
    public StageSlot stageSlotPrefab;
    public RectTransform[] slotParents;
    public GameObject deckUI;
    private AssetListTable assetListTable;
    private StringTable stringTable;
    private GameManager gameManager;
    private bool slotsCreated = false;
    public GameObject stageMask;
    public GameObject deckMask;
    public GameObject dragMask;

    private void Awake()
    {
        gameManager = GameManager.instance;
    }
    
    
    private void OnEnable()
    {
        if (stageTable == null)                     
        {
            stageTable = DataTableManager.Get<StageTable>(DataTableIds.Stage);
            assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
            stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);
        }
        
        if (!slotsCreated)
        {
            CreateStageSlots();
            slotsCreated = true;
        }
        
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
                Logger.Log($"Creating batch for parent {parentIndex} with stages: {string.Join(", ", batch.Select(s => s.StageId))}");
                CreateBatch(batch, slotParents[parentIndex]);
                batch.Clear();
                parentIndex = (parentIndex + 1) % slotParents.Length;
            }
        }
        
        if (batch.Count > 0)
        {
            Logger.Log($"Creating final batch for parent {parentIndex} with stages: {string.Join(", ", batch.Select(s => s.StageId))}");
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
            slot.SetStageMask(stageMask);
            slot.GameManager(gameManager);
            slot.SetDragMask(dragMask);
            slot.SetDeckMask(deckMask);
        }
    }
    
}