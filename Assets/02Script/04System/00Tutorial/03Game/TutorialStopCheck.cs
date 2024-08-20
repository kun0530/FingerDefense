using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStopCheck : TutorialBase
{
    public TutorialObserver tutorialObserver;
    [SerializeField]private MonsterController monster;
    public override void Enter()
    {
        monster=tutorialObserver.GetComponent<MonsterController>();
    }

    public override void Execute(TutorialController controller)
    {
        if(monster.isPaused)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        
    }
}
