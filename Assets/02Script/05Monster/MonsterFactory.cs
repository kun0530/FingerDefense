using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using System;
using Object = UnityEngine.Object;


public class MonsterFactory
{
    public Transform poolTransform;
    private AssetListTable assetList;
    private MonsterTable monsterTable;
    
    private Dictionary<int,IObjectPool<MonsterController>> monsterPool =
        new Dictionary<int, IObjectPool<MonsterController>>();
    
    public void Init(HashSet<int> ids)
    {
        assetList = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        monsterTable = DataTableManager.Get<MonsterTable>(DataTableIds.Monster);
        foreach (var id in ids)
        {
            monsterPool[id] = new ObjectPool<MonsterController>(
                () => CreatedPooledMonster(id),
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyPoolObject,
                true, 10, 1000
            );
        }
    }
    private MonsterController CreatedPooledMonster(int id)
    {
        if (assetList == null)
        {
            throw new InvalidOperationException("AssetList가 초기화되지 않았습니다..");
        }

        var assetId = monsterTable.Get(id).AssetNo;
        var assetFileName = assetList.Get(assetId);
        if (string.IsNullOrEmpty(assetFileName))
        {
            throw new InvalidOperationException($"ID :{assetId}에 대한 자산 경로를 찾을 수 없습니다.");
        }
        
        var assetPath = $"Prefab/03MonsterGame/{assetFileName}";

        // Log the asset path to debug the issue
        Logger.Log($"경로에서 MonsterController 로드하기: {assetPath}");
        
        //TO-DO: Addressable로 변경 예정
        //var monster=Addressables.LoadAssetAsync<MonsterController>(assetPath).WaitForCompletion();
        var monster = Resources.Load<MonsterController>(assetPath);
        if (monster == null)
        {
            throw new InvalidOperationException($"경로에서 MonsterController를 로드하지 못했습니다.: {assetPath}");
        }

        var instantiatedMonster = Object.Instantiate(monster);
        if (poolTransform != null)
            instantiatedMonster.transform.SetParent(poolTransform);

        var monsterData = monsterTable.Get(id);
        instantiatedMonster.Status.Data = monsterData;

        var deathSkill = SkillFactory.CreateSkill(monsterData.Skill, instantiatedMonster.gameObject);
        instantiatedMonster.deathSkill = deathSkill;
        var dragDeathSkill = SkillFactory.CreateSkill(monsterData.DragSkill, instantiatedMonster.gameObject);
        instantiatedMonster.dragDeathSkill = dragDeathSkill;

        instantiatedMonster.pool = monsterPool[id];
        return instantiatedMonster;
    }
    private void OnTakeFromPool(MonsterController monster)
    {
        monster.gameObject.SetActive(true);
    }

    private void OnReturnToPool(MonsterController monster)
    {
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(MonsterController monster)
    {
        Object.Destroy(monster);
    }

    public MonsterController GetMonster(MonsterData data)
    {
        var id = data.Id; 
        if (!monsterPool.ContainsKey(id))
        {
            throw new InvalidOperationException($"몬스터 ID에 대한 풀을 찾을 수 없습니다. {id}");
        }

        var monster = monsterPool[id].Get();
        monster.Status.Data = data;
        monster.ResetMonsterData();

        return monster;
    }
}
