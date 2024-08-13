using UnityEngine;

[RequireComponent(typeof(DialogSystem))]
public class TutorialDialog : TutorialBase
{
    private DialogSystem dialogSystem;

    public override void Enter()
    {
        dialogSystem = GetComponent<DialogSystem>();
        dialogSystem.DialogSetting();
        dialogSystem.isFirstDialog = true;
        dialogSystem.dialogCanvasGroup.gameObject.transform.SetAsLastSibling();
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