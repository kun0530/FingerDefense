using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNickCheck : TutorialBase
{
    
    public override void Enter()
    {
        GameManager.instance.GameData.NicknameCheck = true;
        DataManager.SaveFile(GameManager.instance.GameData);
    }

    public override void Execute(TutorialController controller)
    {
        if (GameManager.instance.GameData.NicknameCheck == true)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        
    }
}
