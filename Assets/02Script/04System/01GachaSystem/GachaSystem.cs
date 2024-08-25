using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

public class GachaSystem : MonoBehaviour
{
    public RectTransform gachaSlotParent;
    public GachaResultSlot resultSlot;
    public PlayableDirector gachaFailDirector;
    public PlayableDirector gachaSuccessDirector;

    private GachaTable gachaTable;
    private AssetListTable assetListTable;
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
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
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
        // 결과 창 초기화
        ClearGachaResults();

        gachaResultPanel.SetActive(false);
        bool isSuccess = false;

        for (int i = 0; i < times; i++)
        {
            GachaData result = GetRandomGachaResult();
            if (result != null)
            {
                bool isNew = true;
                foreach (var id in GameManager.instance.GameData.ObtainedGachaIDs)
                {
                    var upgradedVersion = upgradeTable.upgradeTable.Values.FirstOrDefault(x => x.UpgradeResultId == result.Id);
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

                    RefreshUI();
                }
                else
                {
                    Logger.Log($"Duplicate Gacha ID: {result.Id}, Grade: {result.Grade}");
                    AddMileage(gachaTable.table[result.Id].Grade);
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
        PrepareAndPlayDirectorAsync(isSuccess ? gachaSuccessDirector : gachaFailDirector).Forget();
    }

    private async UniTaskVoid PrepareAndPlayDirectorAsync(PlayableDirector director)
    {
        VideoPlayer videoPlayer = director.GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
            videoPlayer.Prepare();  
            ResetRenderTexture(videoPlayer.targetTexture);
            
            // 비디오 준비가 완료될 때까지 무조건 대기
            await PrepareVideoAsync(videoPlayer);

            videoPlayer.Play();
            director.time = 0;  // 타임라인을 처음부터 재생
            director.Play();
        }
        else
        {
            // 비디오 플레이어가 없을 경우 바로 타임라인 재생
            Logger.LogWarning("비디오 플레이어가 없습니다. 타임라인을 직접 재생합니다.");
            director.time = 0;  // 타임라인을 처음부터 재생
            director.Play();
        }
    }

    private async UniTask PrepareVideoAsync(VideoPlayer videoPlayer)
    {
        var tcs = new UniTaskCompletionSource();

        void OnPrepared(VideoPlayer vp)
        {
            vp.prepareCompleted -= OnPrepared;
            tcs.TrySetResult();
        }

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.Prepare();

        await tcs.Task;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        ShowGachaResultsAfterCutscene();
    }

    private void ShowGachaResultsAfterCutscene()
    {
        gachaResultPanel.SetActive(true);
        ShowGachaResults();
    }

    private GachaData GetRandomGachaResult()
    {
        float rand = Random.value * 100f;
        int grade = (rand <= highGradeProbability) ? 2 :
            (rand <= highGradeProbability + midGradeProbability) ? 1 : 0;

        List<GachaData> possibleResults = gachaTable.table.Values.Where(g => g.Grade == grade).ToList();
        int index = Random.Range(0, possibleResults.Count);
        return possibleResults[index];
    }

    private void SpawnResultSlot(GachaData data, bool isNew)
    {
        var slot = Instantiate(resultSlot, gachaSlotParent);
        slot.Setup(data, assetListTable, stringTable, isNew);
        slot.gameObject.SetActive(false);
        spawnedSlots.Add(slot);
        DataManager.SaveFile(GameManager.instance.GameData);
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

    private void RefreshUI()
    {
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

    private void AddMileage(int grade)
    {
        int mileage = grade switch
        {
            0 => 100,
            1 => 300,
            2 => 500,
            _ => 0
        };

        GameManager.instance.GameData.Mileage += mileage;
    }
    private void ResetRenderTexture(RenderTexture renderTexture)
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            renderTexture.Create();
        }
    }
}
