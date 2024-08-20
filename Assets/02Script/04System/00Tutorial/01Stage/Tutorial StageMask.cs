using DG.Tweening;
using UnityEngine;

public class TutorialStageMask : TutorialBase
{
    public GameObject mask;
    public StagePanelController stagePanelController;
    
    public override void Enter()
    {
        mask.SetActive(true);
        mask.transform.SetAsLastSibling();
        DOTween.Sequence()
            .AppendInterval(stagePanelController.animationDuration) // 애니메이션 지속 시간만큼 대기
            .AppendCallback(() =>
            {
                stagePanelController.enabled = false;
            });
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
           
    }
}
