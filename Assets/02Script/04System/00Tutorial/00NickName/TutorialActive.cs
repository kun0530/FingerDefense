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
            
            if(obj.activeSelf)
                obj.transform.SetAsLastSibling();
            else 
                obj.transform.SetAsFirstSibling();
        }
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }
    
    
    
}
