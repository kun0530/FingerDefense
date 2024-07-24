using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnTest : MonoBehaviour
{
    public MonsterController monsterPrefab;
    public MonsterData monsterData { get; private set; }
    public Transform monsterPos;
    private Vector2 monsterSpawnPos;
    [SerializeField] private float monsterSpawnRadius = 2.5f;

    private void Awake()
    {
        monsterData = new()
        {
            Hp = 100f,
            DragType = 1,
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

        var skill = SkillFactory.CreateSkill(monsterData.Skill, monster.gameObject);
        if (skill == null)
            return;
        var attack = monster.gameObject.AddComponent<PlayerAttackBehavior>();
        attack.baseSkill = skill;
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