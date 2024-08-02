using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;
    
    private TutorialBase currentTutorial=null;
    private int currentTutorialIndex = -1;

    private void Start()
    {
        SetNextTutorial();
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
        }    
        
        if(currentTutorialIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }
        
        currentTutorialIndex++;
        currentTutorial = tutorials[currentTutorialIndex];
        
        currentTutorial.Enter();
    }

    private void CompletedAllTutorials()
    {
        currentTutorial = null;
        
        //해당 게임 오브젝트를 비활성화
        gameObject.SetActive(false);
    }
}
