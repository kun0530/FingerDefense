using UnityEngine;

public class MainUiManager : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject StageUI;
    public GameObject DeckUI;
    public GameObject NicknameUI;
    
    public QuitUI QuitUI;
    
    public void Start()
    {
        MainUI.SetActive(false);
        StageUI.SetActive(false);
        DeckUI.SetActive(false);
        NicknameUI.SetActive(false);
        QuitUI.gameObject.SetActive(false);
    }
    
    
    public void OnClickStartButton()
    {
        StageUI.SetActive(true);
        
    }
    
}
