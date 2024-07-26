using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class TutorialDeckStep
{
    public string title;
    public string description;
    public string buttonText;
}

public class DeckUITutorialManager : MonoBehaviour
{
    public GameObject modalPrefab;
    public GameObject canvas;
    public TutorialDeckStep[] tutorialSteps;

    public int currentStep;
    private GameManager gameManager;
    private ModalWindow currentModal;
    private Action onComplete;

    private GameObject DeckUI;
    public Button BackButton;
    public Button GameButton;
    
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager")?.GetComponent<GameManager>();
    }

    public void StartTutorial(Action onCompleteAction)
    {
        onComplete = onCompleteAction;
        currentStep = 0;
        ShowNextStep();
        BackButton.interactable = false;
        GameButton.interactable = false;
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

        TutorialDeckStep step = tutorialSteps[currentStep];

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
        BackButton.interactable = true;
        GameButton.interactable = true;
        Destroy(gameObject);
        gameManager.DeckUITutorialCheck = true;
    }
}