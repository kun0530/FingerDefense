using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;
    
    private TutorialBase currentTutorial=null;
    private int currentTutorialIndex = -1;
    private GameManager gameManager;
    
    private void Start()
    {     
        foreach (var tutorial in tutorials)
        {
            tutorial.gameObject.SetActive(false);
        }
        SetNextTutorial();
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (currentTutorial)
        {
            currentTutorial.Execute(this);
        }
    }
    
    public void SetNextTutorial()
    {
        if (currentTutorial)
        {
            currentTutorial.Exit();
            currentTutorial.gameObject.SetActive(false);
        }    
        
        if(currentTutorialIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }
        
        currentTutorialIndex++;
        currentTutorial = tutorials[currentTutorialIndex];
        
        var dialogSystem = currentTutorial.GetComponent<DialogSystem>();
        if (dialogSystem != null)
        {
            dialogSystem.isFirstDialog = true;
            dialogSystem.DialogSetting();  // 대화 초기화
        }
        
        currentTutorial.gameObject.SetActive(true);
        currentTutorial.Enter();
    }

    private void CompletedAllTutorials()
    {
        currentTutorial = null;
        DataManager.SaveFile(gameManager.GameData);
        //해당 게임 오브젝트를 비활성화
        gameObject.SetActive(false);
        
    }
}
