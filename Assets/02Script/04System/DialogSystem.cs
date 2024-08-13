using UnityEngine;
using Spine.Unity;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

[System.Serializable]
public struct SystemDialog
{
    public SkeletonGraphic skeletonGraphic;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
}

[System.Serializable]
public struct DialogData
{
    public int speakerIndex;
    public string name;
    
    [TextArea(3, 5)]
    public string dialog;
}

public class DialogSystem : MonoBehaviour
{
    private StringTable stringTable;
    [SerializeField] 
    private SystemDialog[] systemDialog;
    [SerializeField] 
    private DialogData[] dialogData;

    [SerializeField]
    private bool isAutoStart = false;

    public bool isFirstDialog = true;
    
    private int currentDialogIndex = -1;
    private int currentSpeakerIndex = 0;
    
    public Button nextButton;
    public RectTransform dialogTextPanel;
    
    private bool isDialogComplete = false;
    public CanvasGroup dialogCanvasGroup;

    private bool isTyping = false; 
    private bool skipToNext = false; 
    private string fullText; 
    private TextMeshProUGUI currentTextMesh; 

    private void Awake()
    {
        stringTable = DataTableManager.Get<StringTable>(DataTableIds.String); 
    }

    private void Start()
    {
        DialogSetting();
        nextButton.onClick.AddListener(() => OnNextButtonClickedWrapper().Forget());
    }

    public void DialogSetting()
    {
        isDialogComplete = false;
        currentDialogIndex = -1;
        for (var i = 0; i < systemDialog.Length; i++)
        {
            SetActiveObjects(systemDialog[i], false);
            if (systemDialog[i].skeletonGraphic)
            {
                systemDialog[i].skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
                systemDialog[i].skeletonGraphic.canvasRenderer.SetAlpha(1);
            }
        }
    }

    private void SetActiveObjects(SystemDialog dialog, bool visible)
    {
        if (dialog.skeletonGraphic)
        {
            dialog.skeletonGraphic.gameObject.SetActive(visible);   
        }
        
        nextButton.gameObject.SetActive(visible);
        dialog.nameText.gameObject.SetActive(visible);
        dialog.dialogText.gameObject.SetActive(visible);
        dialogTextPanel.gameObject.SetActive(visible);

        if (!visible)
        {
            dialog.dialogText.text = "";
            dialog.nameText.text = "";
        }
    }

    public bool UpdateDialog()
    {
        if (isFirstDialog)
        {
            DialogSetting();

            if (isAutoStart)
            {
                isFirstDialog = false;
                SetNextDialogAsync().Forget();
            }
        }

        return isDialogComplete;
    }
    
    private async UniTaskVoid OnNextButtonClickedWrapper()
    {
        await OnNextButtonClicked();
    }

    private async UniTask OnNextButtonClicked()
    {
        if (isTyping)
        {
            currentTextMesh.text = fullText;
            isTyping = false;
            skipToNext = true;
        }
        else
        {
            await SetNextDialogAsync();
        }
    }

    private async UniTask SetNextDialogAsync()
    {
        foreach (var dialog in systemDialog)
        {
            dialog.dialogText.text = "";
            dialog.nameText.text = "";
            SetActiveObjects(dialog, false);
        }

        currentDialogIndex++;
        
        if (currentDialogIndex >= dialogData.Length)
        {
            isDialogComplete = true;
            dialogCanvasGroup.gameObject.SetActive(false);
            return;
        }

        currentSpeakerIndex = dialogData[currentDialogIndex].speakerIndex;

        SetActiveObjects(systemDialog[currentSpeakerIndex], true);
        systemDialog[currentSpeakerIndex].nameText.text = dialogData[currentDialogIndex].name;
        
        await TypeText(systemDialog[currentSpeakerIndex].dialogText, dialogData[currentDialogIndex].dialog);
    }

    private async UniTask TypeText(TextMeshProUGUI textMesh, string text)
    {
        isTyping = true;
        fullText = text; 
        currentTextMesh = textMesh; 
        textMesh.text = ""; 
        foreach (var t in text)
        {
            if (skipToNext)
            {
                skipToNext = false;
                break;
            }
            textMesh.text += t;
            await UniTask.Delay(50);
        }
        isTyping = false;
    }
}
