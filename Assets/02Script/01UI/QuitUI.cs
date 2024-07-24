using UnityEngine;

public class QuitUI : MonoBehaviour
{
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    
    public void OnClickQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
