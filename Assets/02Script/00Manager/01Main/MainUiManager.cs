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
                if (!GameManager.instance.GameData.MonsterDragLevel.Exists(x =>
                        x.monsterId == upgradeData.UpgradeResultId))
                {
                    GameManager.instance.GameData.MonsterDragLevel.Add((upgradeData.UpgradeResultId,
                        (int)GameData.MonsterDrag.LOCK));
                    DataManager.SaveFile(GameManager.instance.GameData);
                    Logger.Log($"MonsterDragLevel added: {upgradeData.UpgradeResultId}, {GameData.MonsterDrag.LOCK}");
                }
            }
            else if (upgradeData.Type == 1)
            {
                // UpStatType에 따른 초기화
                switch (upgradeData.UpStatType)
                {
                    case 0: // MonsterGimmick ATTACKRANGE
                        if (!gameManager.GameData.MonsterGimmickLevel.Exists(x =>
                                x.monsterGimmick == (int)GameData.MonsterGimmick.ATTACKRANGE))
                        {
                            gameManager.GameData.MonsterGimmickLevel.Add(((int)GameData.MonsterGimmick.ATTACKRANGE, 0));
                            Logger.Log($"MonsterGimmickLevel added: {GameData.MonsterGimmick.ATTACKRANGE}, 0");
                            
                        }

                        break;

                    case 1: // MonsterGimmick ATTACKDAMAG
                        if (!gameManager.GameData.MonsterGimmickLevel.Exists(x =>
                                x.monsterGimmick == (int)GameData.MonsterGimmick.ATTACKDAMAGE))
                        {
                            gameManager.GameData.MonsterGimmickLevel.Add(((int)GameData.MonsterGimmick.ATTACKDAMAGE, 0));
                            Logger.Log($"MonsterGimmickLevel added: {GameData.MonsterGimmick.ATTACKDAMAGE}, 0");
                        }

                        break;

                    case 2: // MonsterGimmick ATTACKDURATION
                        if (!gameManager.GameData.MonsterGimmickLevel.Exists(x =>
                                x.monsterGimmick == (int)GameData.MonsterGimmick.ATTACKDURATION))
                        {
                            gameManager.GameData.MonsterGimmickLevel.Add(((int)GameData.MonsterGimmick.ATTACKDURATION,
                                0));
                            Logger.Log($"MonsterGimmickLevel added: {GameData.MonsterGimmick.ATTACKDURATION}, 0");
                        }

                        break;
                }
            }
            else if (upgradeData.Type == 3)
            {
                // UpStatType에 따른 PlayerUpgrade 초기화
                switch (upgradeData.UpStatType)
                {
                    case 3: // PlayerUpgrade CHARACTER_ARRANGEMENT
                        if (!gameManager.GameData.PlayerUpgradeLevel.Exists(x =>
                                x.playerUpgrade == (int)GameData.PlayerUpgrade.CHARACTER_ARRANGEMENT))
                        {
                            gameManager.GameData.PlayerUpgradeLevel.Add((
                                (int)GameData.PlayerUpgrade.CHARACTER_ARRANGEMENT, 0));
                            Logger.Log($"PlayerUpgradeLevel added: {GameData.PlayerUpgrade.CHARACTER_ARRANGEMENT}, 0");
                        }

                        break;

                    case 4: // PlayerUpgrade PLAYER_HEALTH
                        if (!gameManager.GameData.PlayerUpgradeLevel.Exists(x =>
                                x.playerUpgrade == (int)GameData.PlayerUpgrade.PLAYER_HEALTH))
                        {
                            gameManager.GameData.PlayerUpgradeLevel.Add(((int)GameData.PlayerUpgrade.PLAYER_HEALTH, 0));
                            Logger.Log($"PlayerUpgradeLevel added: {GameData.PlayerUpgrade.PLAYER_HEALTH}, 0");
                        }

                        break;

                    case 5: // PlayerUpgrade INCREASE_DRAG
                        if (!gameManager.GameData.PlayerUpgradeLevel.Exists(x =>
                                x.playerUpgrade == (int)GameData.PlayerUpgrade.INCREASE_DRAG))
                        {
                            gameManager.GameData.PlayerUpgradeLevel.Add(((int)GameData.PlayerUpgrade.INCREASE_DRAG, 0));
                            Logger.Log($"PlayerUpgradeLevel added: {GameData.PlayerUpgrade.INCREASE_DRAG}, 0");
                        }

                        break;
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
