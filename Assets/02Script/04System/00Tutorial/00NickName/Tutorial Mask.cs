using UnityEngine;
using UnityEngine.UI;

public class TutorialMask : TutorialBase
{
    public GameObject Mask;
    public Button NextButton;
    
    private bool isListenerAdded = false;
    private TutorialController tutorialController;
    
    public override void Enter()
    {
        Mask.gameObject.SetActive(true);
        Mask.gameObject.transform.SetAsLastSibling();
        if (!isListenerAdded)
        {
            NextButton.onClick.AddListener(OnNextButtonClicked);
            isListenerAdded = true;
        }
    }

    private void OnNextButtonClicked()
    {
        Exit();
        tutorialController.SetNextTutorial();
    }

    public override void Execute(TutorialController controller)
    {
        tutorialController = controller;    
    }

    public override void Exit()
    {
        Mask.gameObject.SetActive(false);
        Mask.gameObject.transform.SetAsFirstSibling();
    }
    
    
}                    
