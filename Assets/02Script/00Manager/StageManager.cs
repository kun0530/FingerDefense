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
    [Header("Castle")]
    public List<GameObject> castleImages;
    public Transform castleRightTopPos;
    public Transform castleLeftBottomPos;

    private float castleMaxHp;
    private float castleHp;
    private float CastleHp
    {
        get => castleHp;
        set
        {
            castleHp = value;
            gameUiManager.UpdateHpBar(castleHp, castleMaxHp);

            if (castleImages.Count != 0)
            {
                int castleIndex = Mathf.FloorToInt(castleHp / (castleMaxHp / castleImages.Count));
                castleIndex = Mathf.Clamp(castleIndex, 0, castleImages.Count - 1);
                for(int i = 0; i < castleImages.Count; i++)
                {
                    castleImages[i].SetActive(i == castleIndex);
                }
            }
        }
    }
    private float castleShield;
    private float CastleShield
    {
        get => castleShield;
        set
        {
            castleShield = value;
            gameUiManager.UpdateShieldBar(castleShield, castleMaxHp);
            if (castleShield > 0f)
                shieldEffect?.gameObject.SetActive(true);
            else
                shieldEffect?.gameObject.SetActive(false);
        }
    }
    [SerializeField] private EffectController shieldEffect;

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
            
            if (currentState == StageState.GameClear &&
                Variables.LoadTable.StageId >= GameManager.instance.GameData.stageClearNum)
            {
                GameManager.instance.GameData.stageClearNum = Variables.LoadTable.StageId;
                Logger.Log($"현재 최고 스테이지 클리어 ID: {Variables.LoadTable.StageId}");
            }

            if (currentState == StageState.GameClear || currentState == StageState.GameOver)
            {
                GameManager.instance.GameData.Gold += EarnedGold;
                DataManager.SaveFile(GameManager.instance.GameData);
            }
        }
    }

    public MonsterSpawner monsterSpawner;
    public PlayerCharacterSpawner playerCharacterSpawner;
    public GameUiManager gameUiManager;

    [HideInInspector] public bool isPlayerElementAdvantage = false;

    private void Awake()
    {
        var upgradesData = GameManager.instance.GameData.PlayerUpgradeLevel;
        var castleMaxHpLevel = 0;
        foreach (var upgradeData in upgradesData)
        {
            if (upgradeData.playerUpgrade == (int)GameData.PlayerUpgrade.PLAYER_HEALTH)
            {
                castleMaxHpLevel = upgradeData.level;
                break;
            }
        }

        var upgradeTableDic = DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade).upgradeTable;
        foreach (var upgradeData in upgradeTableDic)
        {
            if (upgradeData.Value.UpStatType == (int)GameData.PlayerUpgrade.PLAYER_HEALTH
                && upgradeData.Value.Level == castleMaxHpLevel)
            {
                castleMaxHp = upgradeData.Value.UpStatValue;
            }
        }
    }

    private void Start()
    {
        CastleHp = castleMaxHp;
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
            heal *= castleMaxHp;
        CastleHp += heal;

        if (CastleHp >= castleMaxHp)
            CastleHp = castleMaxHp;
    }

    public void GetShield(float shield, bool isPercentage = false)
    {
        if (shield <= 0f)
            return;

        if (isPercentage)
            shield *= castleMaxHp;
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
