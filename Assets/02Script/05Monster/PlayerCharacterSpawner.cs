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
    private AssetListTable assetListTable;
    private PlayerCharacterData[] playerCharacters = new PlayerCharacterData[8]; // 사용할 캐릭터 데이터들
    private PlayerCharacterController[] activePlayerCharacters = new PlayerCharacterController[6]; // 현재 활성화된 캐릭터 저장

    private void Awake()
    {
        playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);

        if (playerCharacterTable == null)
        {
            Logger.LogError("PlayerCharacterTable is not initialized.");
            return;
        }
        if (assetListTable == null)
        {
            Logger.LogError("AssetListTable is not initialized.");
            return;
        }

        for (var i = 0; i < Variables.LoadTable.characterIds.Length; i++)
        {
            if (Variables.LoadTable.characterIds[i] != 0)
            {
                playerCharacters[i] = playerCharacterTable.Get(Variables.LoadTable.characterIds[i]);
            }
        }
    }

    private PlayerCharacterController CreatePlayerCharacter(PlayerCharacterData data)
    {
        if (data == null)
        {
            Logger.LogError("Character data is null.");
            return null;
        }

        var assetName = assetListTable.Get(data.AssetNo);
        if (string.IsNullOrEmpty(assetName))
        {
            Logger.LogError($"Asset name not found for AssetNo {data.AssetNo}");
            return null;
        }

        var prefab = Resources.Load<GameObject>($"Prefab/02CharacterGame/{assetName}");
        if (prefab == null)
        {
            Logger.LogError($"Prefab not found for asset name {assetName}");
            return null;
        }

        var playerCharacter = Instantiate(prefab, poolTransform).GetComponent<PlayerCharacterController>();
        if (playerCharacter == null)
        {
            Logger.LogError($"PlayerCharacterController not found on prefab {assetName}");
            return null;
        }
        playerCharacter.Status.Data = data;
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
        var data = playerCharacters[index];

        if (data == null)
        {
            Logger.LogError("해당 캐릭터 데이터가 없습니다.");
            return;
        }

        var playerCharacter = CreatePlayerCharacter(data);

        if (playerCharacter == null) // To-Do: 리스폰 쿨타임 조건 추가
            return;

        var spawnClass = playerCharacter.Status.Data.Class;
        Logger.Log($"Spawning character with Class {spawnClass}");

        var positionIndex = spawnClass switch
        {
            // 전열
            0 => activePlayerCharacters[0] == null || !activePlayerCharacters[0]?.gameObject.activeSelf == true ? 0 : 1,
            // 중열
            1 => activePlayerCharacters[2] == null || !activePlayerCharacters[2]?.gameObject.activeSelf == true ? 2 : 3,
            // 후열
            2 => activePlayerCharacters[4] == null || !activePlayerCharacters[4]?.gameObject.activeSelf == true ? 4 : 5,
            _ => -1
        };

        if (positionIndex == -1 || positionIndex >= spawnPositions.Length)
        {
            Logger.LogError("해당 위치에 캐릭터를 배치할 수 없습니다.");
            return;
        }

        // 캐릭터 위치 설정 후 활성화
        playerCharacter.transform.position = spawnPositions[positionIndex].position;

        activePlayerCharacters[positionIndex] = playerCharacter;
        playerCharacter.ResetPlayerData();
        playerCharacter.gameObject.SetActive(true);
        // To-Do: 초기화

        // playerCharacterSpawner.SpawnPlayerCharacter(index);
    }

    public void RemoveActiveCharacter(PlayerCharacterController character)
    {
        for (var i = 0; i < activePlayerCharacters.Length; i++)
        {
            if (activePlayerCharacters[i] == character)
            {
                activePlayerCharacters[i] = null;
            }
        }
    }
}