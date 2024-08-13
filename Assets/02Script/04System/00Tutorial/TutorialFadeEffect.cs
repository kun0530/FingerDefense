using UnityEngine;

public class TutorialFadeEffect : TutorialBase
{
    [SerializeField]
    private FadeEffect fadeEffect;
    [SerializeField]
    private bool isFadeIn = false;
    private bool isComplete = false;

    public override void Enter()
    {
        fadeEffect.gameObject.transform.SetAsLastSibling();
        fadeEffect.gameObject.SetActive(true);
        fadeEffect.Initialize(isFadeIn ? 0 : 1);
        
        if (isFadeIn)
        {
            fadeEffect.FadeIn(OnAfterFadeEffect);
        }
        else
        {
            fadeEffect.FadeOut(OnAfterFadeEffect);
        }
        
    }
    
    private void OnAfterFadeEffect()
    {
        isComplete = true;
    }
    
    public override void Execute(TutorialController controller)
    {
        if (isComplete)
        {
            controller.SetNextTutorial();
        }    
    }

    public override void Exit()
    {
    }
}
