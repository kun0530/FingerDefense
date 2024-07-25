using System;
using UnityEngine;

[Serializable]
public class TutorialStageStep
{
    public string title;
    public string description;
    public string buttonText;
}
public class StageUITutorialManager : MonoBehaviour
{
    public GameObject modalPrefab;
    public GameObject canvas;
    public TutorialStageStep[] tutorialSteps;
    
    public int currentStep;
    private GameManager gameManager;
    private ModalWindow currentModal;
    private Action onComplete;
    
    private GameObject StageUI;
    
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager")?.GetComponent<GameManager>();
    }
    
    public void StartTutorial(Action onCompleteAction)
    {
        onComplete = onCompleteAction;
        currentStep = 0;
        ShowNextStep();
    }
    
    private void ShowNextStep()
    {
        if (currentModal != null)
        {
            Destroy(currentModal.gameObject);
        }
        
        if (currentStep >= tutorialSteps.Length)
        {
            EndTutorial();
            return;
        }
        
        TutorialStageStep step = tutorialSteps[currentStep];
        
        currentModal = ModalWindow.Create(modalPrefab, canvas);
        currentModal.SetHeader(step.title);
        currentModal.SetBody(step.description);
        currentModal.SetButton(step.buttonText, OnModalButtonClick);
        currentModal.Show();
    }
    
    private void OnModalButtonClick()
    {
        currentStep++;
        ShowNextStep();
    }
    
    private void EndTutorial()
    {
        onComplete?.Invoke();
        Destroy(gameObject);
        gameManager.StageChoiceTutorialCheck = true;
    }
    
}
