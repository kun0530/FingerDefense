using UnityEngine;

public class TutorialStageMask : TutorialBase
{
    public GameObject mask;
    public StagePanelController stagePanelController;
    
    public override void Enter()
    {
        stagePanelController.enabled = true;
        mask.SetActive(true);
        mask.transform.SetAsLastSibling();
        
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
           
    }
}
