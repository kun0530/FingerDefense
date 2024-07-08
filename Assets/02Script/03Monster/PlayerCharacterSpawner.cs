using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterSpawner : MonoBehaviour
{
    public PlayerCharacterController characterPrefab; // To-Do: 추후 AssetReference로 변경
    public Transform poolTransform;
    public Transform[] spawnPositions = new Transform[6];

    private PlayerCharacterTable playerCharacterTable;

    private void Awake()
    {
        playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
    }

    private void Update()
    {

    }

    public PlayerCharacterController SpawnPlayerCharacter(int id, int index)
    {
        var data = playerCharacterTable.Get(id);
        // To-Do: data에 따른 캐릭터 분기
        var playerCharacter = GameObject.Instantiate(characterPrefab);
        playerCharacter.Data = data;
        return playerCharacter;
    }

    public PlayerCharacterController SpawnPlayerCharacter(PlayerCharacterData data, int index)
    {
        // To-Do: data에 따른 캐릭터 분기
        var playerCharacter = GameObject.Instantiate(characterPrefab);
        playerCharacter.transform.position = spawnPositions[index].position;
        playerCharacter.transform.SetParent(poolTransform);
        playerCharacter.Data = data;
        return playerCharacter;
    }
}