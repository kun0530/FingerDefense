using System;
using UnityEngine;


[Serializable]
public class TutorialGameStep
{
    public string title;
    public string description;
    public string buttonText;
}
public class GameTutorialManager : MonoBehaviour
{
    public GameObject modalPrefab;
    public GameObject canvas;
    public TutorialGameStep[] tutorialSteps;
    
    public int currentStep = 0;
    private GameManager gameManager;
    private ModalWindow currentModal;
    private Action onComplete;

    
    public static event Action OnTutorialComplete;
    
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
    
    void Start()
    {
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
        
        TutorialGameStep step = tutorialSteps[currentStep];
        
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
        OnTutorialComplete?.Invoke();
        gameManager.GameTutorialCheck = true;
        Destroy(gameObject);
        Time.timeScale = 1f;
    }
}
