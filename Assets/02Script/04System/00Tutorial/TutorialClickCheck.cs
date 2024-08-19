using UnityEngine;
using Cysharp.Threading.Tasks;

public class TutorialClickCheck : TutorialBase
{
    public GameObject target;
    
    private bool isTargetActivated = false;
    
    public override void Enter()
    {
        isTargetActivated = false;
        CheckTarget().Forget();
    }

    private async UniTaskVoid CheckTarget()
    {
        while (!isTargetActivated)
        {
            if (target.activeSelf)
            {
                isTargetActivated = true;
            }
            await UniTask.Yield();
        }
    }

    public override void Execute(TutorialController controller)
    {
        if (isTargetActivated)
        {
            controller.SetNextTutorial();
        }    
    }

    public override void Exit()
    {
        
    }
}
