using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnTest : MonoBehaviour
{
    public MonsterController monsterPrefab;
    private MonsterData monsterData;
    public Transform monsterPos;
    private Vector2 monsterSpawnPos;
    [SerializeField] private float monsterSpawnRadius = 2.5f;

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

    private void Start()
    {
        monsterSpawnPos = new Vector2(monsterPos.transform.position.x, monsterPos.transform.position.y);
    }

    public void SpawnMonster()
    {
        var spawnPos = monsterSpawnPos + Random.insideUnitCircle * monsterSpawnRadius;
        var monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
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