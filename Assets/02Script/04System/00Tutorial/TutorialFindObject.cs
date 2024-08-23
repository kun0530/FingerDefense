using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFindObject : TutorialBase
{
    private GameObject findObject;
    private bool isClick = false;
    
    public override void Enter()
    {
        findObject= GameObject.FindWithTag("Modal");
        if (findObject != null)
        {
            findObject.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                isClick = true;
            });
        }
    }

    public override void Execute(TutorialController controller)
    {
        if (isClick)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        
    }
}

