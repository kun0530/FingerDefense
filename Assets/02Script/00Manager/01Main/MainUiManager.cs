using UnityEngine;

public class MainUiManager : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject StageUI;
    public GameObject DeckUI;
    public QuitUI QuitUI;
    
    public void Start()
    {
        MainUI.SetActive(true);
        StageUI.SetActive(false);
        DeckUI.SetActive(false);
        QuitUI.gameObject.SetActive(false);
    }
    
    
}
