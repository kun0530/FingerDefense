using UnityEngine.SceneManagement;

public class TutorialSceneLoad : TutorialBase
{
    private UpgradeTable upgradeTable;
    
    private void Start()
    {
        upgradeTable ??= DataTableManager.Get<UpgradeTable>(DataTableIds.Upgrade);
    }
    
    public override void Enter()
    {
        if(Variables.LoadTable.StageId == 13001)
        {
            GameManager.instance.GameData.Game1TutorialCheck = true;
            GameManager.instance.GameData.StageChoiceTutorialCheck = true;
            GameManager.instance.GameData.ObtainedGachaIDs.Add(300470);
            //GameManager.insstance.GameData.StageClear[13001]=true;
            DataManager.SaveFile(GameManager.instance.GameData);
        }

        if(Variables.LoadTable.StageId == 13002)
        {
            GameManager.instance.GameData.Game2TutorialCheck = true;
            GameManager.instance.GameData.DeckUITutorialCheck = true;
            GameManager.instance.GameData.ObtainedGachaIDs.Add(300596);
            //12031,0에 해당하는 것을 1로 바꾸기 
            GameManager.instance.GameData.MonsterDragLevel[12031] = (int)GameData.MonsterDrag.UNLOCK;
            GameManager.instance.GameData.Gold += 100;
            DataManager.SaveFile(GameManager.instance.GameData);
        }
        
        if(Variables.LoadTable.StageId == 13003)
        {
            GameManager.instance.GameData.ShopDragTutorialCheck = true;
            GameManager.instance.GameData.Game3TutorialCheck = true; 
            GameManager.instance.GameData.ObtainedGachaIDs.Add(300530);
            DataManager.SaveFile(GameManager.instance.GameData);
        }        
    }

    public override void Execute(TutorialController controller)
    {
        DataManager.SaveFile(GameManager.instance.GameData);    
        controller.SetNextTutorial();
        SceneManager.LoadScene(1);
    }

    public override void Exit()
    {
    }
}


