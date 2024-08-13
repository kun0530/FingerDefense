using UnityEngine;

public class TutorialActive : TutorialBase
{
    [SerializeField]
    private GameObject[] tutorialObject;
    private GameManager gameManager;
    private MainUI mainUI;
    
    public void Awake()
    {
        gameManager = GameManager.instance;
    }
    
    public override void Enter()
    {
        foreach (var obj in tutorialObject)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
        gameManager.ResourceManager.NicknameCheck = true;

        if (mainUI)
        {
            mainUI.UpdatePlayerInfo();    
        }
        
    }

    public override void Exit()
    {
        
    }
    
   
}
