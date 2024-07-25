using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum StageState
{
    None, Playing, GameOver, GameClear
}

public class StageManager : MonoBehaviour
{
    public float CastleMaxHp { get; private set; } = 100f;
    private float castleHp;
    public float CastleHp
    {
        get => castleHp;
        private set
        {
            castleHp = value;
            gameUiManager.UpdateHpBar(castleHp, CastleMaxHp);
        }
    }

    private int monsterCount;
    public int MonsterCount
    {
        get => monsterCount;
        set
        {
            monsterCount = value;
            gameUiManager.UpdateMonsterCount(monsterCount);
            if (monsterCount <= 0 && CastleHp > 0f)
                CurrentState = StageState.GameClear;
        }
    }

    private int earnedGold;
    public int EarnedGold
    {
        get => earnedGold;
        set
        {
            earnedGold = value;
            gameUiManager.UpdateEarnedGold(earnedGold);
        }
    }
    
    private StageState currentState;
    public StageState CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState == value || value == StageState.None)
                return;

            currentState = value;
            gameUiManager.SetStageStateUi(currentState);
            Time.timeScale = currentState is StageState.GameClear or StageState.GameOver  ? 0f : 1f;
        }
    }

    public MonsterSpawner monsterSpawner;
    public PlayerCharacterSpawner playerCharacterSpawner;
    public GameUiManager gameUiManager;

    private void Start()
    {
        CastleHp = CastleMaxHp;
        CurrentState = StageState.Playing;
        MonsterCount = monsterSpawner.MonsterCount;
        EarnedGold = 0;
        
        
    }

    public void DamageCastle(float damage)
    {
        if (damage <= 0f)
            return;

        CastleHp -= damage;

        if (CastleHp <= 0f)
            CurrentState = StageState.GameOver;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    
    public void LobbyScene()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
}
