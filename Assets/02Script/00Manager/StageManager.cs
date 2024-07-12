using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum StageState
{
    Game, GameOver, GameClear
}

public class StageManager : MonoBehaviour
{
    private float castleMaxHp = 100f;
    public float CastleHp { get; private set; }
    public Image castleHpBar;
    public GameObject gameOverUi;
    public GameObject gameClearUi;
    public GameObject gameUi;
    public TextMeshProUGUI monsterCountText;
    private int monsterCount;
    public int MonsterCount
    {
        get { return monsterCount; }
        set
        {
            monsterCount = value;
            monsterCountText.text = $"Monster: {monsterCount}";
            if (monsterCount <= 0 && CastleHp > 0f)
            {
                GameClear();
            }
        }
    }
    public StageState State { get; private set; }



    public MonsterSpawner monsterSpawner;
    public PlayerCharacterSpawner playerCharacterSpawner;



    private void Awake()
    {
        CastleHp = castleMaxHp;
        State = StageState.Game;
    }

    private void Start()
    {
        gameUi.SetActive(true);
        gameOverUi.SetActive(false);
        gameClearUi.SetActive(false);
        MonsterCount = monsterSpawner.MonsterCount;
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
        gameUi.SetActive(false);
        gameClearUi.SetActive(true);
        State = StageState.GameClear;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        gameUi.SetActive(false);
        gameOverUi.SetActive(true);
        State = StageState.GameOver;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
