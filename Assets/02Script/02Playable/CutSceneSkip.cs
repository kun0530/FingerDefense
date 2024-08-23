using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutSceneSkip : MonoBehaviour
{
    public PlayableDirector gachaFailDirector;
    public PlayableDirector gachaSuccessDirector;
    
    public Button skipButton;
    
    public void Start()
    {
        skipButton.onClick.AddListener(SkipCutScene);
    }

    private void SkipCutScene()
    {
        if (gachaFailDirector.state == PlayState.Playing)
        {
            gachaFailDirector.time = gachaFailDirector.duration;
        }
        else if (gachaSuccessDirector.state == PlayState.Playing)
        {
            gachaSuccessDirector.time = gachaSuccessDirector.duration;
        }
    }
}
