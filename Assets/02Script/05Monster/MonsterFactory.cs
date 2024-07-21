using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using System;
using Object = UnityEngine.Object;

// [Serializable]
// public class AssetReferenceMonster : AssetReferenceT<MonsterController>
// {
//     public AssetReferenceMonster(string guid) : base(guid) {}
// }

public class MonsterFactory
{
    // public AssetReferenceMonster monsterPrefab;
    public MonsterController monsterPrefab; // To-Do: 추후 삭제
    private IObjectPool<MonsterController> poolMonster;

    public Transform poolTransform;

    public void Init()
    {
        poolMonster = new ObjectPool<MonsterController>(
            CreatedPooledMonster,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true, 10, 1000
        );
    }

    private MonsterController CreatedPooledMonster()
    {
        // var monster = Instantiate(monsterPrefab.Asset as MonsterController);
        var monster = Object.Instantiate(monsterPrefab);
        if (poolTransform != null)
            monster.transform.SetParent(poolTransform);
        monster.pool = poolMonster;
        return monster;
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
        // To-Do: 몬스터 데이터 처리
        var monster =  poolMonster.Get();

        var monsterStatus = new MonsterStatus(data);
        monster.Status = monsterStatus;
        monster.ResetMonsterData();

        return monster;
    }
}
