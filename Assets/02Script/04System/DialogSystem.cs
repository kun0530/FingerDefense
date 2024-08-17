using Coffee.UIExtensions;
using UnityEngine;
using Spine.Unity;
using TMPro;
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
    public int? dialogId;
    public string dialog;
    
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    
}

public class DialogSystem : MonoBehaviour
{
    private StringTable stringTable;
    [SerializeField] internal SystemDialog[] systemDialog;
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
            //SetActiveObjects(systemDialog[i], false);
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
        else if (!isDialogComplete)
        {
            // 타이핑이 완료된 후, 다음 대화를 시작
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
        
        var gameObj = dialogData[currentDialogIndex];
                SetObjectsActive(gameObj.objectsToActivate, true);
                SetObjectsActive(gameObj.objectsToDeactivate, false);
        
        var dialogText = dialogData[currentDialogIndex].dialogId.HasValue 
            ? stringTable.Get(dialogData[currentDialogIndex].dialogId.Value.ToString())
            : GetDialogText(dialogData[currentDialogIndex].dialog);


        await TypeText(systemDialog[currentSpeakerIndex].dialogText, dialogText);  // 수정된 부분
    }

    private string GetDialogText(string dialog)
    {
        return int.TryParse(dialog, out int dialogId) ?
            // 숫자로 변환 가능하면 StringTable에서 가져옴
            stringTable.Get(dialogId.ToString()) :
            // 그렇지 않으면 그대로 반환
            dialog;
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
    
    private void SetObjectsActive(GameObject[] targetObjects, bool isActive)
    {
        if (targetObjects != null)
        {
            foreach (var obj in targetObjects)
            {
                if (obj)
                {
                    obj.SetActive(isActive);
                    if (isActive)
                    {
                        // 오브젝트를 부모의 맨 뒤에서 두 번째로 이동
                        int lastSiblingIndex = obj.transform.parent.childCount - 1;
                        obj.transform.SetSiblingIndex(lastSiblingIndex - 1);
                    }
                }
            }
        }
    }
}
