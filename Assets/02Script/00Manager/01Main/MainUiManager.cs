using UnityEngine;

public class MainUiManager : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject StageUI;
    public GameObject DeckUI;
    public QuitUI QuitUI;
    
    public void Start()
    {
        MainUI.SetActive(false);
        StageUI.SetActive(true);
        DeckUI.SetActive(false);
        QuitUI.gameObject.SetActive(false);
        
    }
    
    
    public void OnClickStartButton()
    {
        StageUI.SetActive(true);
        
    }
    
}
