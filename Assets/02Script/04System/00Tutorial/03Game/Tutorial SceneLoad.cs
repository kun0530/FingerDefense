using UnityEngine.SceneManagement;

public class TutorialSceneLoad : TutorialBase
{
    public override void Enter()
    {
        if(Variables.LoadTable.StageId == 13001)
        {
            GameManager.instance.GameData.Game1TutorialCheck = true;
            GameManager.instance.GameData.StageChoiceTutorialCheck = true;
            GameManager.instance.GameData.ObtainedGachaIDs.Add(300470);
            DataManager.SaveFile(GameManager.instance.GameData);
        }

        if(Variables.LoadTable.StageId == 13002)
        {
            GameManager.instance.GameData.Game2TutorialCheck = true;
            GameManager.instance.GameData.ObtainedGachaIDs.Add(300596);
        }
        
        if(Variables.LoadTable.StageId == 13003)
        {
            GameManager.instance.GameData.Game3TutorialCheck = true;    
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


