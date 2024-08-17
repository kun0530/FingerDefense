using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStageMask : TutorialBase
{
    public GameObject mask;
    
    public override void Enter()
    {
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
