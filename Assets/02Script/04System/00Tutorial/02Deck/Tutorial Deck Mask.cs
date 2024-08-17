using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;

public class TutorialDeckMask : TutorialMask
{
    public GameObject mask;
    public Unmask unmask;

    public RectTransform targetMask;
    public override void Enter()
    {
        mask.SetActive(true);
        unmask.fitTarget=targetMask;
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

