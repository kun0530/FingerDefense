using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public PlayerCharacterSpawner playerCharacterSpawner;

    private PlayerCharacterData[] playerCharactersOfData = new PlayerCharacterData[6];
    private PlayerCharacterController[] activePlayerCharacters = new PlayerCharacterController[6];

    private void Awake()
    {
        var playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        // PlayerCharacterData 배열을 초기에 받는다

        // To-Do: 임시로 할당, 나중에 덱 편성으로부터 받아야 함
        playerCharactersOfData[0] = playerCharacterTable.Get(101);
        playerCharactersOfData[1] = playerCharacterTable.Get(102);
        playerCharactersOfData[2] = playerCharacterTable.Get(103);
    }

    public void SpawnPlayerCharacter(int index)
    {
        // 버튼 클릭
        // 해당 버튼의 쿨타임 확인
        // 
        var spawnPriority = playerCharactersOfData[index].Priority;
        if (activePlayerCharacters[spawnPriority * 2] == null)
        {
            activePlayerCharacters[spawnPriority * 2] = playerCharacterSpawner.SpawnPlayerCharacter(playerCharactersOfData[index], spawnPriority * 2);
        }
        else if (activePlayerCharacters[spawnPriority * 2 + 1] == null)
        {
            activePlayerCharacters[spawnPriority * 2 + 1] = playerCharacterSpawner.SpawnPlayerCharacter(playerCharactersOfData[index], spawnPriority * 2 + 1);
        }
        else
        {
            Logger.Log("모든 자리가 차있습니다.");
        }
        
        // playerCharacterSpawner.SpawnPlayerCharacter(index);
    }
}
