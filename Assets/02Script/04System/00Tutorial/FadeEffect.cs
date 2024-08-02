using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeEffect : MonoBehaviour
{
    private Image BackgroundImage;
    
    [Range(0.01f, 10f)]
    [SerializeField]
    private float fadeTime = 2.0f;
    
    [SerializeField]
    private AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    private void Awake()
    {
        BackgroundImage = GetComponent<Image>();
    }
    public void Initialize(float initialAlpha)
    {
        // 초기 알파 값 설정
        Color color = BackgroundImage.color;
        color.a = initialAlpha;
        BackgroundImage.color = color;
    }
    public void FadeIn(Action onAfterFadeEffect)
    {
        gameObject.SetActive(true);
        BackgroundImage.DOFade(1, fadeTime).SetEase(fadeCurve).OnComplete(() => 
        {
            onAfterFadeEffect();
            gameObject.SetActive(false);
        });
    }

    public void FadeOut(Action onAfterFadeEffect)
    {
        gameObject.SetActive(true);
        BackgroundImage.DOFade(0, fadeTime).SetEase(fadeCurve).OnComplete(() => 
        {
            onAfterFadeEffect();
            gameObject.SetActive(false);
        });
    }
}
