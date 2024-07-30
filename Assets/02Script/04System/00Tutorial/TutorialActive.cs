using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActive : TutorialBase
{
    [SerializeField]
    private GameObject tutorialObject;
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
    }

    public override void Exit()
    {
    }
}
