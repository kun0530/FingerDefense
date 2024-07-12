using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUiManager : MonoBehaviour
{
    private StageManager stageManager;

    private Dictionary<StageState, GameObject> stageStatesUi = new Dictionary<StageState, GameObject>();
    public GameObject gameUi;
    public GameObject gameOverUi;
    public GameObject gameClearUi;

    public TextMeshProUGUI monsterCountText;

    public Image castleHpBar;

    private void Start()
    {
        stageManager = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();

        stageStatesUi.Add(StageState.Playing, gameUi);
        stageStatesUi.Add(StageState.GameOver, gameOverUi);
        stageStatesUi.Add(StageState.GameClear, gameClearUi);
    }

    public void SetStageStateUi(StageState state)
    {
        foreach (var stageStateUi in stageStatesUi)
        {
            if (state == stageStateUi.Key)
                stageStateUi.Value.SetActive(true);
            else
                stageStateUi.Value.SetActive(false);
        }
    }

    public void UpdateHpBar(float currentHp, float maxHp)
    {
        castleHpBar.fillAmount = currentHp / maxHp;
    }

    public void UpdateMonsterCount(int monsterCount)
    {
        monsterCountText.text = $"Monster: {monsterCount}";
    }
}
