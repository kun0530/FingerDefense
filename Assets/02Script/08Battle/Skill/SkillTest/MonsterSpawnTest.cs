using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnTest : MonoBehaviour
{
    public MonsterController monsterPrefab;
    private MonsterData monsterData;
    public Transform monsterPos;

    private void Awake()
    {
        monsterData = new()
        {
            Hp = 100f,
            DragType = 0,
            Element = 0,
            MoveSpeed = 5f,
            AtkDmg = 0f,
            AtkSpeed = 1f,
            Height = 3f,
            Skill = 0
        };
    }

    public void SpawnMonster()
    {
        var monster = Instantiate(monsterPrefab, monsterPos.position, Quaternion.identity);
        monster.Status.Data = monsterData;
    }

    public void RemoveAllMonster()
    {
        var monsters = GameObject.FindGameObjectsWithTag(Defines.Tags.MONSTER_TAG);
        foreach (var monster in monsters)
        {
            Destroy(monster);
        }
    }
}