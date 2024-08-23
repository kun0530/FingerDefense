using UnityEngine;

public class QuitUI : MonoBehaviour
{
    private StageManager stageManager;

    private void Start()
    {
        if(stageManager == null)
            stageManager = GameObject.FindWithTag("StageManager").TryGetComponent(out StageManager manager) ? manager : null;
    }
    
    public void ResumeGame()
    {
        if (stageManager.CurrentState == StageState.GAME_OVER)
        {
            return;
        }
        else
        {
            TimeScaleController.SetTimeScale(1f);    
        }
    }
    
    public void OnClickQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
