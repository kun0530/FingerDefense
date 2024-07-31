using UnityEngine;

public class TutorialActive : TutorialBase
{
    [SerializeField]
    private GameObject tutorialObject;
    GameManager gameManager;
    
    public void Awake()
    {
        gameManager = GameManager.instance;
    }
    
    public override void Enter()
    {
        if (tutorialObject.activeSelf == false)
        {
            tutorialObject.SetActive(true);
        }
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
        gameManager.NicknameCheck = true;
    }

    public override void Exit()
    {
    }
}
