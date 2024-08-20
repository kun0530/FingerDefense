using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSave : TutorialBase
{
    public TutorialController saveObject;
    
    public override void Enter()
    {
       
    }

    public override void Execute(TutorialController controller)
    {
        if (saveObject.gameObject.CompareTag("ShopDrag"))
        {
            GameManager.instance.GameData.ShopDragTutorialCheck = true;
            DataManager.SaveFile(GameManager.instance.GameData);
            controller.SetNextTutorial();
            SceneManager.LoadScene(1);
            Logger.Log("저장 완료");
        }
        if (saveObject.gameObject.CompareTag("ShopGimmick"))
        {
            GameManager.instance.GameData.ShopGimmickTutorialCheck = true;
            DataManager.SaveFile(GameManager.instance.GameData);
            controller.SetNextTutorial();
            Logger.Log("저장 완료");
        }
        if (saveObject.gameObject.CompareTag("ShopCharacter"))
        {
            GameManager.instance.GameData.ShopCharacterTutorialCheck = true;
            DataManager.SaveFile(GameManager.instance.GameData);
            controller.SetNextTutorial();
            Logger.Log("저장 완료");
        }
        if (saveObject.gameObject.CompareTag("ShopStage"))
        {
            GameManager.instance.GameData.ShopFeatureTutorialCheck = true;
            DataManager.SaveFile(GameManager.instance.GameData);
            controller.SetNextTutorial();
            Logger.Log("저장 완료");
        }
        else
        {
            Logger.LogError("TutorialSave.cs: Enter() - saveObject is not tagged");
        }
    }

    public override void Exit()
    {
        
    }
}

