using UnityEngine;

public class TutorialActive : TutorialBase
{
    [SerializeField]
    private GameObject[] tutorialObject;
    private GameManager gameManager;
    
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
    }

    public override void Exit()
    {
    }
    
   
}
