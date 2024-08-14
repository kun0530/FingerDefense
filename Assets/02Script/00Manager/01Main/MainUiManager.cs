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
    public UpgradePanelManager upgradePanelManager;
    
    public TutorialController tutorialController;
    public TutorialController stageTutorialController;
    
    private UpgradeTable upgradeTable;
    private void Awake()
    {
        upgradeTable ??= DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
        gameManager = GameManager.instance;
    }
    public void Start()
    {
        if (!gameManager.GameData.NicknameCheck)
        {
            tutorialController.gameObject.SetActive(true);
            MainUI.gameObject.SetActive(false);
            DeckUI.SetActive(false);
            StageUI.SetActive(false);
            NicknameUI.SetActive(false);
            ShopUI.gameObject.SetActive(false);
            GachaSystem.gameObject.SetActive(false);
            upgradePanelManager.gameObject.SetActive(false);
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
            upgradePanelManager.gameObject.SetActive(false);
        }

        foreach (var upgradeData in upgradeTable.upgradeTable.Values)
        {
            if (upgradeData.Type == 0)
            {
                if (!GameManager.instance.GameData.MonsterDragLevel.Exists(x => x.monsterId == upgradeData.UpgradeResultId))
                {
                    GameManager.instance.GameData.MonsterDragLevel.Add((upgradeData.UpgradeResultId, (int)GameData.MonsterDrag.LOCK));
                    DataManager.SaveFile(GameManager.instance.GameData);
                    Logger.Log($"MonsterDragLevel added: {upgradeData.UpgradeResultId}, {GameData.MonsterDrag.LOCK}");
                }    
            }

            if (upgradeData.Type == 1)
            {
                if(upgradeData.UpStatType == 0)
                {
                    
                }
                else if(upgradeData.UpStatType == 1)
                {
                    
                }
                else if(upgradeData.UpStatType == 2)
                {
                    
                }
            }
            
            
            
        }
        
    }
    
    
    public void OnClickStartButton()
    {
        if (!StageUI.activeSelf)
        {
            StageUI.SetActive(true);
            
            if(!gameManager.GameData.StageChoiceTutorialCheck)
            {
                stageTutorialController.gameObject.SetActive(true);
            }
        }
    }
    
}
