using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterSpawner : MonoBehaviour
{
    public PlayerCharacterController[] characterPrefabs; // To-Do: 추후 AssetReference로 변경
    public Transform poolTransform;
    public Transform[] spawnPositions; // 6개 (전열: 0, 1 / 중열: 2, 3 / 후열: 4, 5)

    private PlayerCharacterTable playerCharacterTable;
    private SkillTable skillTable;

    private PlayerCharacterController[] playerCharacters = new PlayerCharacterController[8]; // 사용할 캐릭터들
    private PlayerCharacterController[] activePlayerCharacters = new PlayerCharacterController[6]; // 현재 활성화된 캐릭터 저장

    private void Awake()
    {
        playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);

        // PlayerCharacterData 배열을 초기에 받는다

        // To-Do: 임시로 할당, 나중에 덱 편성으로부터 받아야 함
        playerCharacters[0] = GeneratePlayerCharacter(100);
        playerCharacters[1] = GeneratePlayerCharacter(101);
        playerCharacters[2] = GeneratePlayerCharacter(102);
        playerCharacters[3] = GeneratePlayerCharacter(103);
        playerCharacters[4] = GeneratePlayerCharacter(104);
        playerCharacters[5] = GeneratePlayerCharacter(105);
        playerCharacters[6] = GeneratePlayerCharacter(106);
        playerCharacters[7] = GeneratePlayerCharacter(107);
    }

    private void Update()
    {

    }

    private PlayerCharacterController GeneratePlayerCharacter(int id)
    {
        var data = playerCharacterTable.Get(id);

        var playerCharacter = Instantiate(characterPrefabs[data.AssetNo], poolTransform, true);
        playerCharacter.Status = new CharacterStatus(data)
        {
            buffHandler = playerCharacter.buffHandler
        };

        // 에러로 인해 비활성화 : 방민호
        // var skillData = skillTable.Get(data.Skill);
        // playerCharacter.skill = SkillFactory.CreateSkill(skillData, playerCharacter.transform);
        // playerCharacter.skillData = skillData;

        playerCharacter.spawner = this;
        playerCharacter.gameObject.SetActive(false);

        return playerCharacter;
    }

    public void SpawnPlayerCharacter(int index)
    {
        // 버튼 클릭
        // 해당 버튼의 쿨타임 확인
        var playerCharacter = playerCharacters[index];

        if (playerCharacter == null || !playerCharacter.IsDead) // To-Do: 리스폰 쿨타임 조건 추가
            return;

        var spawnPriority = playerCharacter.Status.data.Priority;
        if (activePlayerCharacters[spawnPriority * 2] == null || !activePlayerCharacters[spawnPriority * 2].gameObject.activeSelf)
        {
            activePlayerCharacters[spawnPriority * 2] = playerCharacter;
            playerCharacter.gameObject.transform.position = spawnPositions[spawnPriority * 2].position;
        }
        else if (activePlayerCharacters[spawnPriority * 2 + 1] == null || !activePlayerCharacters[spawnPriority * 2 + 1].gameObject.activeSelf)
        {
            activePlayerCharacters[spawnPriority * 2 + 1] = playerCharacter;
            playerCharacter.gameObject.transform.position = spawnPositions[spawnPriority * 2 + 1].position;
        }
        else
        {
            Logger.Log("모든 자리가 차있습니다.");
            return;
        }

        playerCharacter.ResetPlayerData();
        playerCharacter.gameObject.SetActive(true);
        // To-Do: 초기화

        // playerCharacterSpawner.SpawnPlayerCharacter(index);
    }

    public void RemoveActiveCharacter(PlayerCharacterController character)
    {
        for (int i = 0; i < activePlayerCharacters.Length; i++)
        {
            if (activePlayerCharacters[i] == character)
            {
                activePlayerCharacters[i] = null;
            }
        }
    }
}