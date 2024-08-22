using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    
    public Button[] stageButtons;
    
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
        SetStartPositionBasedOnClearedStage();
        for (var i = 0; i < stageButtons.Length; i++)
        {
            var index = i; // 람다식 캡처 문제를 피하기 위해 지역 변수를 사용
            stageButtons[i].onClick.AddListener(() => OnStageButtonClicked(index));
        }
        
    }

    private void SetStartPositionBasedOnClearedStage()
    {
        int lastClearedStage = 0;
        var stageClear = gameManager.GameData.StageClear;

        foreach (var kvp in stageClear)
        {
            if (kvp.Value)
            {
                lastClearedStage = kvp.Key;
            }
        }

        // 모든 slotParents를 활성화 (모두 보이도록 함)
        foreach (var parent in slotParents)
        {
            parent.gameObject.SetActive(true);
        }

        // 특정 스테이지가 클리어되었을 때 다음 패널 활성화
        if (lastClearedStage == 13005)
        {
            ActivatePanel(1);  // stageButtons[1]과 대응하는 패널 활성화
        }
        else if (lastClearedStage == 13010)
        {
            ActivatePanel(2);  // stageButtons[2]과 대응하는 패널 활성화
        }
        else if (lastClearedStage == 13015)
        {
            ActivatePanel(3);  // stageButtons[3]과 대응하는 패널 활성화
        }
        else if (lastClearedStage == 13020)
        {
            ActivatePanel(4);  // stageButtons[4]과 대응하는 패널 활성화
        }
        else
        {
            // 기본으로 첫 번째 패널을 활성화
            ActivatePanel(0);
        }
    }
    private void ActivatePanel(int panelIndex)
    {
        // 모든 slotParents를 비활성화
        foreach (var parent in slotParents)
        {
            parent.gameObject.SetActive(false);
        }

        // 해당 인덱스의 패널만 활성화
        if (panelIndex >= 0 && panelIndex < slotParents.Length)
        {
            slotParents[panelIndex].gameObject.SetActive(true);
        }

        // 해당 인덱스의 버튼을 활성화 (잠금 해제)
        if (panelIndex >= 0 && panelIndex < stageButtons.Length)
        {
            stageButtons[panelIndex].interactable = true;
        }
    }
    private void OnStageButtonClicked(int index)
    {
        // 특정 스테이지가 클리어되었는지 확인 (5, 10, 15, 20)
        var isStage5Cleared = gameManager.GameData.StageClear.TryGetValue(13005, out var stage5Cleared) && stage5Cleared;
        var isStage10Cleared = gameManager.GameData.StageClear.TryGetValue(13010, out var stage10Cleared) && stage10Cleared;
        var isStage15Cleared = gameManager.GameData.StageClear.TryGetValue(13015, out var stage15Cleared) && stage15Cleared;
        var isStage20Cleared = gameManager.GameData.StageClear.TryGetValue(13020, out var stage20Cleared) && stage20Cleared;

        // 해당 index에 맞는 slotParents를 활성화할 수 있는지 여부 확인
        bool canActivate = false;
    
        if (index == 0)
        {
            canActivate = true;
        }
        else if (index == 1 && isStage5Cleared)
        {
            canActivate = true;
        }
        else if (index == 2 && isStage10Cleared)
        {
            canActivate = true;
        }
        else if (index == 3 && isStage15Cleared)
        {
            canActivate = true;
        }
        else if (index == 4 && isStage20Cleared)
        {
            canActivate = true;
        }

        // 패널을 비활성화하기 전에 조건을 체크
        if (canActivate)
        {
            // 모든 slotParents를 비활성화
            foreach (var parent in slotParents)
            {
                parent.gameObject.SetActive(false);
            }

            // 해당 index에 맞는 slotParents만 활성화
            if (index >= 0 && index < slotParents.Length)
            {
                slotParents[index].gameObject.SetActive(true);
            }
        }
        else
        {
            // 조건을 만족하지 않는 경우, 모달 창을 띄우고 기존 패널을 유지
            ModalWindow.Create()
                .SetHeader("잠금")
                .SetBody("해당 스테이지를 모두 클리어해야 합니다.")
                .AddButton("확인", () => { })
                .Show();
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

            // 현재 스테이지가 클리어되었는지 확인
            bool isStageCleared = gameManager.GameData.StageClear.TryGetValue(stageData.StageId, out bool isCleared) && isCleared;

            // 이전 스테이지가 클리어되었는지 확인
            bool isPreviousStageCleared = true;
            if (stageData.StageId > 1)
            {
                isPreviousStageCleared = gameManager.GameData.StageClear.TryGetValue(stageData.StageId - 1, out bool previousCleared) && previousCleared;
            }

            // 슬롯 활성화 여부 설정
            slot.gameObject.SetActive(true);  // 슬롯은 모두 활성화
            
        }
    }
}