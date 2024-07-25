using System;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    public GameObject modalPrefab;
    public GameObject canvas;
    public TutorialStep[] tutorialSteps;
    
    public int currentStep = 0;
    private GameManager gameManager;
    private ModalWindow currentModal;
    private Action onComplete;
    
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
    
    void Start()
    {
        if (gameManager !=null || !gameManager.GameTutorialCheck)
        {
            StartTutorial(() => { Time.timeScale = 0f; });
        }
    }
    private void StartTutorial(Action onCompleteAction)
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
        
        TutorialStep step = tutorialSteps[currentStep];
        
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
        gameManager.GameTutorialCheck = true;
        Time.timeScale = 1f;
    }
}
