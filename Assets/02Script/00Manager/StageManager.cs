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



    private void Awake()
    {
        CastleHp = castleMaxHp;
    }

    private void Start()
    {
        gameOverText.enabled = false;
        gameClearText.enabled = false;
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
        gameClearText.enabled = true;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        gameOverText.enabled = true;
    }
}
