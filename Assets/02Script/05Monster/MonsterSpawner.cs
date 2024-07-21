using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MonsterSpawner : MonoBehaviour
{
    private MonsterFactory factory;

    public MonsterController monsterPrefab; // 추후 AssetReferenceMonster로 교체
    public Transform poolTransform;
    public Transform[] spawnPositions;

    private MonsterTable monsterTable;
    private WaveTable waveTable;

    public Transform moveTarget;

    private int stageId = Defines.LoadTable.stageId; // 테스트용. 나중에 다른 클래스의 static 변수로 변경.
    private int waveId = 1;
    public int MonsterCount { get; private set; }

    public float spawnGroupInterval = 0.5f;
    private float spawnWaveTimer = 0f;
    private WaveData currentWaveData;
    private bool isNextWave;

    private bool isWaveEnd;


    private void Awake()
    {
        factory = new MonsterFactory();
        factory.monsterPrefab = monsterPrefab;
        factory.Init();

        monsterTable = DataTableManager.Get<MonsterTable>(DataTableIds.Monster);
        waveTable = DataTableManager.Get<WaveTable>(DataTableIds.Wave);

        // 스테이지 매니저의 내용
        // 스테이지 내 등장 몬스터 수(잘못된 id 몬스터는 나오지 않는걸로)
        // 죽거나 포털로 들어가면 카운트 다운 0
        int wave = 1;
        MonsterCount = 0;
        WaveData waveData;
        while ((waveData = waveTable.Get(stageId, wave++))!= null)
        {
            var monsterList = waveData.monsters;
            foreach (var monster in monsterList)
            {
                MonsterCount += monster.monsterCount;
            }
        }
        Logger.Log($"{stageId}스테이지의 몬스터 수: {MonsterCount}");

        currentWaveData = waveTable.Get(stageId, waveId);
        if (currentWaveData != null)
        {
            spawnWaveTimer = currentWaveData.Term;
            isNextWave = true;
        }
    }

    private void OnEnable()
    {
        isWaveEnd = false;
    }

    private void Start()
    {
        factory.poolTransform = poolTransform;
    }

    private void Update()
    {
        if (MonsterCount <= 0 || !isNextWave || isWaveEnd)
            return;
        
        if (spawnWaveTimer >= currentWaveData.Term)
        {
            spawnWaveTimer = 0f;
            isNextWave = false;
            SpawnMonsterGroupUniTask().Forget();
        }
        else
        {
            spawnWaveTimer += Time.deltaTime;
        }
    }

    private async UniTask SpawnMonsterGroupUniTask()
    {
        // var monsters = currentWaveData.monsters;
        // foreach (var monster in monsters)
        // {
        //     for (int i = 0; i < monster.monsterCount; i++)
        //     {
        //         var monsterGo = factory.GetMonster(monsterTable.Get(monster.monsterId));
        //         monsterGo.transform.position = spawnPositions[monster.monsterCount + i - 1].position;
        //         monsterGo.moveTarget = moveTarget;
        //     }
        //     await UniTask.WaitForSeconds(spawnGroupInterval);
        // }
        // waveId++;
        // isNextWave = true;
        //
        // currentWaveData = waveTable.Get(stageId, waveId);
        // if (currentWaveData == null)
        // {
        //     isNextWave = false;
        //     isWaveEnd = true;
        // }
        
        var monsters = currentWaveData.monsters;
        foreach (var monster in monsters)
        {
            for (int i = 0; i < monster.monsterCount; i++)
            {
                var monsterGo = factory.GetMonster(monsterTable.Get(monster.monsterId));
                int spawnIndex = (monster.monsterCount + i - 1) % spawnPositions.Length;
                monsterGo.transform.position = spawnPositions[spawnIndex].position;
                monsterGo.moveTarget = moveTarget;
            }
            await UniTask.WaitForSeconds(spawnGroupInterval);
        }
        waveId++;
        isNextWave = true;

        currentWaveData = waveTable.Get(stageId, waveId);
        if (currentWaveData == null)
        {
            isNextWave = false;
            isWaveEnd = true;
        }
        else
        {
            Debug.Log($"Next wave: {waveId}, Monster Count: {currentWaveData.monsters.Count}");
        }
    }
}
