using UnityEngine;

[RequireComponent(typeof(DialogSystem))]
public class TutorialDialog : TutorialBase
{
    private DialogSystem dialogSystem;

    public override void Enter()
    {
        dialogSystem = GetComponent<DialogSystem>();
        dialogSystem.DialogSetting();    
    }

    public override void Execute(TutorialController controller)
    {
        bool isComplete = dialogSystem.UpdateDialog();
        
        if (isComplete)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
    }
}