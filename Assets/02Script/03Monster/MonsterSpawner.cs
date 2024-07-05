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

    private int stageId = 1; // 테스트용. 나중에 다른 클래스의 static 변수로 변경.
    private int waveId = 1;
    private int monsterCount;

    public float spawnGroupInterval = 0.5f;
    private float spawnTimer = 0f;
    private WaveData currentWaveData;
    private bool isNextWave;


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
        monsterCount = 0;
        WaveData waveData;
        while ((waveData = waveTable.Get(stageId, wave++))!= null)
        {
            var monsterList = waveData.monsters;
            foreach (var monster in monsterList)
            {
                monsterCount += monster.monsterCount;
            }
        }
        Logger.Log($"{stageId}스테이지의 몬스터 수: {monsterCount}");

        currentWaveData = waveTable.Get(stageId, waveId);
        if (currentWaveData != null)
        {
            spawnTimer = currentWaveData.Term;
            isNextWave = true;
        }
    }

    private void Start()
    {
        factory.poolTransform = poolTransform;
    }

    private void Update()
    {
        if (monsterCount <= 0 || !isNextWave)
            return;

        if (spawnTimer >= currentWaveData.Term)
        {
            spawnTimer = 0f;
            isNextWave = false;
            SpawnMonsterGroup().Forget();
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
    }

    private async UniTask SpawnMonsterGroup()
    {
        var monsters = currentWaveData.monsters;
        foreach (var monster in monsters)
        {
            for (int i = 0; i < monster.monsterCount; i++)
            {
                var monsterGo = factory.GetMonster(monsterTable.Get(monster.monsterId));
                monsterGo.transform.position = spawnPositions[monster.monsterCount + i - 1].position;
                monsterCount--;
            }
            await UniTask.WaitForSeconds(spawnGroupInterval);
        }
        waveId++;
        isNextWave = true;
    }
}
