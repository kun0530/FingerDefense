using UnityEngine;

public class TutorialStageMask : TutorialBase
{
    public GameObject mask;
    public StagePanelController stagePanelController;
    
    public override void Enter()
    {
        mask.SetActive(true);
        mask.transform.SetAsLastSibling();
        stagePanelController.enabled = true;
    }

    public override void Execute(TutorialController controller)
    {
        stagePanelController.enabled = false;
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
 
    }
}
