using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    private float castleMaxHp = 100f;
    public float CastleHp { get; private set; }
    public Image castleHpBar;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameClearText;
    public TextMeshProUGUI monsterCountText;
    private int monsterCount;
    public int MonsterCount
    {
        get { return monsterCount; }
        set
        {
            monsterCount = value;
            monsterCountText.text = $"Monster: {monsterCount}";
            if (monsterCount <= 0)
            {
                GameClear();
            }
        }
    }

    public MonsterSpawner monsterSpawner;
    public PlayerCharacterSpawner playerCharacterSpawner;

    private PlayerCharacterData[] playerCharactersOfData = new PlayerCharacterData[6];
    private PlayerCharacterController[] activePlayerCharacters = new PlayerCharacterController[6];

    private void Awake()
    {
        CastleHp = castleMaxHp;

        var playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        // PlayerCharacterData 배열을 초기에 받는다

        // To-Do: 임시로 할당, 나중에 덱 편성으로부터 받아야 함
        playerCharactersOfData[0] = playerCharacterTable.Get(101);
        playerCharactersOfData[1] = playerCharacterTable.Get(102);
        playerCharactersOfData[2] = playerCharacterTable.Get(103);
    }

    private void Start()
    {
        gameOverText.enabled = false;
        gameClearText.enabled = false;
        MonsterCount = monsterSpawner.MonsterCount;
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

    public void DamageCastle(float damage)
    {
        if (damage <= 0f)
            return;

        CastleHp -= damage;
        UpdateHpBar();

        if (CastleHp <= 0f)
        {
            GameOver();
        }
    }

    private void UpdateHpBar()
    {
        var hpPercent = CastleHp / castleMaxHp;
        castleHpBar.fillAmount = hpPercent;
    }

    private void GameClear()
    {
        Time.timeScale = 0f;
        gameClearText.enabled = true;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        gameOverText.enabled = true;
    }
}
