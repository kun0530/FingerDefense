using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialFadeEffect : TutorialBase
{
    [SerializeField]
    private FadeEffect fadeEffect;
    [SerializeField]
    private bool isFadeIn = false;
    private bool isComplete = false;

    public override void Enter()
    {
        if(!fadeEffect.gameObject.activeSelf)
        {
            fadeEffect.gameObject.SetActive(true);
        }
        
        fadeEffect.Initialize(isFadeIn ? 0 : 1);
        
        if (isFadeIn == true)
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
        if (isComplete == true)
        {
            controller.SetNextTutorial();
        }    
    }

    public override void Exit()
    {
        fadeEffect.gameObject.SetActive(false);
    }
}
