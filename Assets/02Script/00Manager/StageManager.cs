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
    private float CastleHp
    {
        get => castleHp;
        set
        {
            castleHp = value;
            gameUiManager.UpdateHpBar(castleHp, CastleMaxHp);
        }
    }
    private float castleShield;
    private float CastleShield
    {
        get => castleShield;
        set
        {
            castleShield = value;
            gameUiManager.UpdateShieldBar(castleShield, CastleMaxHp);
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
    [HideInInspector] public float goldMultiplier = 1f;
    
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
            TimeScaleController.SetTimeScale(currentState is StageState.GameClear or StageState.GameOver ? 0f : 1f);
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

        if (CastleShield > 0f)
        {
            if (CastleShield > damage)
            {
                CastleShield -= damage;
                return;
            }
            else
            {
                damage = CastleShield - damage;
                CastleShield = 0f;
            }
        }

        CastleHp -= damage;

        if (CastleHp <= 0f)
            CurrentState = StageState.GameOver;
    }

    public void RestoreCastle(float heal, bool isPercentage = false)
    {
        if (heal <= 0f)
            return;

        if (isPercentage)
            heal *= CastleMaxHp;
        CastleHp += heal;

        if (CastleHp >= CastleMaxHp)
            CastleHp = CastleMaxHp;
    }

    public void GetShield(float shield, bool isPercentage = false)
    {
        if (shield <= 0f)
            return;

        if (isPercentage)
            shield *= CastleMaxHp;
        CastleShield += shield;
    }

    public void GetGold(int gold)
    {
        EarnedGold += Mathf.CeilToInt(gold * goldMultiplier);
        gameUiManager.UpdateEarnedGold(earnedGold);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        TimeScaleController.SetTimeScale(1f);
    }
    
    public void LobbyScene()
    {
        SceneManager.LoadScene(1);
        TimeScaleController.SetTimeScale(1f);
    }
}
