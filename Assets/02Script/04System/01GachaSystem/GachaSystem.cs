using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GachaSystem : MonoBehaviour
{
    public RectTransform gachaSlotParent;
    public GachaResultSlot resultSlot;
    public PlayableDirector gachaFailDirector;
    public PlayableDirector gachaSuccessDirector;

    private GachaTable gachaTable;
    private AssetListTable aasetListTable;
    private StringTable stringTable;
    private UpgradeTable upgradeTable;

    public Button closeButton;
    private readonly List<GachaResultSlot> spawnedSlots = new List<GachaResultSlot>();

    [SerializeField, Range(0f, 10f), Tooltip("고급 등급 확률이 해당 값에 따라서 변경됩니다."), Header("고급 등급 확률")]
    private float highGradeProbability = 3f;

    [SerializeField, Range(0f, 30f), Tooltip("중급 등급 확률이 해당 값에 따라서 변경됩니다."), Header("중급 등급 확률")]
    private float midGradeProbability = 20f;

    public GameObject gachaResultPanel;

    private void Start()
    {
        gachaTable = DataTableManager.Get<GachaTable>(DataTableIds.Gacha);
        aasetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);
        upgradeTable = DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClearGachaResults);
        }

        gachaFailDirector.stopped += OnTimelineFinished;
        gachaSuccessDirector.stopped += OnTimelineFinished;
    }

    public void PerformGacha(int times)
    {
        gachaResultPanel.SetActive(false);
        bool isSuccess = false; // 성공 여부를 추적하는 변수

        for (int i = 0; i < times; i++)
        {
            GachaData result = GetRandomGachaResult();
            if (result != null)
            {
                bool isNew = true;
                foreach (var id in GameManager.instance.GameData.ObtainedGachaIDs)
                {
                    var upgradedVersion =
                        upgradeTable.upgradeTable.Values.FirstOrDefault(x => x.UpgradeResultId == result.Id);
                    if (id == result.Id || (upgradedVersion != null && id == upgradedVersion.NeedCharId))
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    GameManager.instance.GameData.ObtainedGachaIDs.Add(result.Id);
                    Logger.Log($"Obtained Gacha ID: {result.Id}, Grade: {result.Grade}");
                    DataManager.SaveFile(GameManager.instance.GameData);

                    CharacterUpgradePanel upgradePanel = FindObjectOfType<CharacterUpgradePanel>();
                    if (upgradePanel != null)
                    {
                        upgradePanel.RefreshPanel();
                    }

                    DeckSlotController deckSlotController = FindObjectOfType<DeckSlotController>();
                    if (deckSlotController != null)
                    {
                        deckSlotController.RefreshCharacterSlots();
                    }
                }
                else
                {
                    // 중복 처리
                    Logger.Log($"Duplicate Gacha ID: {result.Id}, Grade: {result.Grade}");
                    switch (gachaTable.table[result.Id].Grade)
                    {
                        case 0:
                            GameManager.instance.GameData.Mileage += 100;
                            break;
                        case 1:
                            GameManager.instance.GameData.Mileage += 300;
                            break;
                        case 2:
                            GameManager.instance.GameData.Mileage += 500;
                            break;
                    }
                }

                // 3성(Grade 2) 결과가 있으면 성공으로 플래그 설정
                if (result.Grade == 2)
                {
                    isSuccess = true;
                    Logger.Log("3성 캐릭터 획득!");
                }

                // 결과 슬롯 생성 (결과는 모든 뽑기마다 생성)
                SpawnResultSlot(result, isNew);
            }
        }

        // 하나라도 3성(Grade 2) 결과가 있으면 성공 컷신, 아니면 실패 컷신
        if (isSuccess)
        {
            Logger.Log("Playing Success Cutscene");
            gachaSuccessDirector.Play();
        }
        else
        {
            Logger.Log("Playing Fail Cutscene");
            gachaFailDirector.Play();
        }
    }

    private GachaData GetRandomGachaResult()
    {
        float rand = Random.value * 100f;
        int grade;

        if (rand <= highGradeProbability)
        {
            grade = 2;
        }
        else if (rand <= highGradeProbability + midGradeProbability)
        {
            grade = 1;
        }
        else
        {
            grade = 0;
        }

        List<GachaData> possibleResults = new List<GachaData>();

        foreach (var gachaData in gachaTable.table.Values)
        {
            if (gachaData.Grade == grade)
            {
                possibleResults.Add(gachaData);
            }
        }

        int index = Random.Range(0, possibleResults.Count);
        return possibleResults[index];
    }

    private void SpawnResultSlot(GachaData data, bool isNew)
    {
        var slot = Instantiate(resultSlot, gachaSlotParent);
        slot.Setup(data, aasetListTable, stringTable, isNew);
        slot.gameObject.SetActive(false);
        spawnedSlots.Add(slot);
        DataManager.SaveFile(GameManager.instance.GameData);
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        gachaResultPanel.SetActive(true);
        ShowGachaResults();
    }

    private void ShowGachaResults()
    {
        foreach (var slot in spawnedSlots)
        {
            slot.gameObject.SetActive(true);
        }
    }

    private void ClearGachaResults()
    {
        foreach (var slot in spawnedSlots)
        {
            Destroy(slot.gameObject);
        }

        spawnedSlots.Clear();
    }
}