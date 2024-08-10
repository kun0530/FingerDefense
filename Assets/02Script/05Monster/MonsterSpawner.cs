using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    private MonsterFactory factory;
    public Transform poolTransform;
    public Transform spawnTransform;
    private Vector2 spawnPosition;
    public float spawnRadius;

    private MonsterTable monsterTable;
    private WaveTable waveTable;

    public Transform moveTarget1;
    public Transform moveTarget2;

    private int stageId = Variables.LoadTable.StageId;
    private int waveId = 1;
    public int MonsterCount { get; private set; }

    private float spawnWaveTimer = 0f;
    private WaveData currentWaveData;
    private bool isWaveTerm;
    [SerializeField] private float waveTerm = 3f;

    private bool isWaveEnd;
    public HashSet<int> monsters { get; private set; } = new();
    
    private StageManager stageManager;

    public event Action<MonsterController> onResetMonster;
    
    private void Awake()
    {
        factory = new MonsterFactory();
        //factory.monsterPrefab = monsterPrefab;
        //factory.Init();

        monsterTable = DataTableManager.Get<MonsterTable>(DataTableIds.Monster);
        waveTable = DataTableManager.Get<WaveTable>(DataTableIds.Wave);

        // 스테이지 매니저의 내용
        // 스테이지 내 등장 몬스터 수(잘못된 id 몬스터는 나오지 않는걸로)
        // 죽거나 포털로 들어가면 카운트 다운 0
        stageId = Variables.LoadTable.StageId; 
        int wave = 1;
        MonsterCount = 0;

        
        while (waveTable.Get(stageId, wave++) is { } waveData)
        {
            MonsterCount += waveData.Repeat;
            foreach (var monster in waveData.monsters)
            {
                monsters.Add(monster.monsterId);
            }
        }
        Logger.Log($"{stageId}스테이지의 몬스터 수: {MonsterCount}");

        factory.Init(monsters);

        currentWaveData = waveTable.Get(stageId, waveId);
        if (currentWaveData != null)
        {
            spawnWaveTimer = 0f;
            isWaveTerm = true;
        }
    }
    
    
    private void OnEnable()
    {
        isWaveEnd = false;
    }

    private void Start()
    {
        stageManager=GameObject.FindWithTag("StageManager").GetComponent<StageManager>();
        
        factory.poolTransform = poolTransform; // To-Do: 추후 삭제
        spawnPosition = new Vector2(spawnTransform.position.x, spawnTransform.position.y);
    }

    private void Update()
    {
        //To-Do:Prototype 이후 !isTutorialCompleted 삭제
        if (MonsterCount <= 0 || !isWaveTerm || isWaveEnd)
            return;

        spawnWaveTimer += Time.deltaTime;
        if (spawnWaveTimer >= waveTerm)
        {
            spawnWaveTimer = 0f;
            isWaveTerm = false;

            if (Time.timeScale != 0)
            {
                SpawnRandomMonster().Forget();    
            }
        }
    }

    private async UniTask SpawnRandomMonster()
    {
        Logger.Log($"현재 웨이브: {waveId}, 이번 몬스터 수: {currentWaveData.Repeat}");
        var repeatCount = 0;
        var monsters = currentWaveData.monsters;
        
        while (repeatCount++ < currentWaveData.Repeat)
        {
            var spwanMonsterId = Utils.WeightedRandomPick(monsters);
            var monsterGo = factory.GetMonster(monsterTable.Get(spwanMonsterId));
            
            if (stageManager)
            {
                monsterGo.transform.position = spawnPosition + Random.insideUnitCircle * spawnRadius;
                monsterGo.moveTargetPos = Utils.GetRandomPositionBetweenTwoPositions(moveTarget1.position, moveTarget2.position);
                monsterGo.ResetMonsterData();
            }
            
            while (Time.timeScale == 0)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(currentWaveData.RepeatTerm));
        }

        isWaveTerm = true;
        waveTerm = currentWaveData.WaveTerm;

        currentWaveData = waveTable.Get(stageId, ++waveId);
        if (currentWaveData == null)
        {
            isWaveTerm = false;
            isWaveEnd = true;
            Logger.Log("모든 몬스터가 소환되었습니다.");
        }
    }

    public void TriggerMonsterReset(MonsterController monster)
    {
        onResetMonster?.Invoke(monster);
    }
}
