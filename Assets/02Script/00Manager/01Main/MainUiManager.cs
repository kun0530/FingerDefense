using UnityEngine;

public class MainUiManager : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject StageUI;
    public GameObject DeckUI;
    public QuitUI QuitUI;
    
    private GameManager gameManager;
    private StageUITutorialManager stageUITutorialManager;
    
    
    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager")?.GetComponent<GameManager>();
        stageUITutorialManager = GameObject.FindGameObjectWithTag("Tutorial")?.GetComponentInChildren<StageUITutorialManager>();
        
        MainUI.SetActive(true);
        StageUI.SetActive(false);
        DeckUI.SetActive(false);
        QuitUI.gameObject.SetActive(false);
    }
    
    
    public void OnClickStartButton()
    {
        StageUI.SetActive(true);
        
        //Prototype 삭제 예정
        if (!gameManager.StageChoiceTutorialCheck)
        {
            stageUITutorialManager.StartTutorial(() => { });
        }
    }
    
}
