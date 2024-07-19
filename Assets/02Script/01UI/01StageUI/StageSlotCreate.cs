using System.Collections.Generic;
using UnityEngine;

public class StageSlotCreate : MonoBehaviour
{
    private StageTable stageTable;
    public StageSlot stageSlotPrefab;
    public RectTransform[] slotParents;
    public GameObject deckUI;
    
    public GameObject[] monsterPrefabs;
    private void Start()
    {
        stageTable ??= DataTableManager.Get<StageTable>(DataTableIds.Stage);

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
            slot.Configure(stageData);
            slot.SetDeckUI(deckUI);
        }
    }
}