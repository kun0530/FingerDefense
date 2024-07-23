using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    public PlayerCharacterController playerCharacterPrefab;
    private PlayerCharacterData playerCharacterData;
    public Transform characterPos;
    private PlayerCharacterController currentPlayerCharacter;

    public MonsterController monsterPrefab;
    private MonsterData monsterData;
    public Transform monsterPos;

    private void Awake()
    {
        playerCharacterData = new()
        {
            Hp = 100f,
            Element = 0,
            Skill1 = 0,
            Skill2 = 0,
            // 사용하지 않음
            AtkDmg = 0f,
            AtkSpeed = 1f,
            AtkRange = 3f
        };

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

    public void CreatePlayerCharacter()
    {
        if (currentPlayerCharacter)
            return;

        var playerCharacter = Instantiate(playerCharacterPrefab, characterPos.position, Quaternion.identity);
        playerCharacter.Status.Data = playerCharacterData;
        currentPlayerCharacter = playerCharacter;
    }

    public void RemovePlayerCharacter()
    {
        if (!currentPlayerCharacter)
            return;
        
        Destroy(currentPlayerCharacter.gameObject);
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