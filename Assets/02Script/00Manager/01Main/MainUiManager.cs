using UnityEngine;

public class MainUiManager : MonoBehaviour
{
    public MainUI MainUI;
    public GameObject StageUI;
    public GameObject DeckUI;
    public GameObject NicknameUI;
    
    public QuitUI QuitUI;
    public ShopSettingUI ShopUI;
    public GachaSystem GachaSystem;
    private GameManager gameManager;
    
    public TutorialController tutorialController;
    public TutorialController stageTutorialController;
    
    
    private void Awake()
    {
        gameManager = GameManager.instance;
    }
    public void Start()
    {
        if (!gameManager.NicknameCheck)
        {
            tutorialController.gameObject.SetActive(true);
            MainUI.gameObject.SetActive(false);
            DeckUI.SetActive(false);
            StageUI.SetActive(false);
            NicknameUI.SetActive(false);
            ShopUI.gameObject.SetActive(false);
            GachaSystem.gameObject.SetActive(false);
        }
        else
        {
            NicknameUI.SetActive(false);
            tutorialController.gameObject.SetActive(false);
            MainUI.gameObject.SetActive(true);
            DeckUI.SetActive(false);
            StageUI.SetActive(false);
            ShopUI.gameObject.SetActive(false);
            GachaSystem.gameObject.SetActive(false);
        }
    }
    
    
    public void OnClickStartButton()
    {
        if (!StageUI.activeSelf)
        {
            StageUI.SetActive(true);
            
            if(!gameManager.StageChoiceTutorialCheck)
            {
                stageTutorialController.gameObject.SetActive(true);
            }
        }
    }
    
}
