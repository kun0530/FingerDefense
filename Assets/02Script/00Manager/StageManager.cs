using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum StageState
{
    Playing, GameOver, GameClear
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
    
    private StageState currentState;
    public StageState CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState == value)
                return;

            currentState = value;
            gameUiManager.SetStageStateUi(currentState);
            if (currentState == StageState.GameClear || currentState == StageState.GameOver)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
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
    }
}
