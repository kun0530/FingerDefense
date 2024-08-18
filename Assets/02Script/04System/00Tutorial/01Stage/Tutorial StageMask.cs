using UnityEngine;

public class TutorialStageMask : TutorialBase
{
    public GameObject mask;
    public StagePanelController stagePanelController;
    
    public override void Enter()
    {
        mask.SetActive(true);
        mask.transform.SetAsLastSibling();
        stagePanelController.enabled = false;
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
        
    }

    public override void Exit()
    {
        
    }
}
