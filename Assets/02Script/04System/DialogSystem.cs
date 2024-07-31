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
    
    private void Awake()
    {
        stringTable = DataTableManager.Get<StringTable>(DataTableIds.String); 
    }

    private void Start()
    {
        DialogSetting();

        nextButton.onClick.AddListener(async () => await OnNextButtonClicked());
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
            else
            {
                Debug.LogWarning("SkeletonGraphic is null");
            }
        }
    }

    private void SetActiveObjects(SystemDialog dialog, bool visible)
    {
        if(!dialog.skeletonGraphic)
        {
            Debug.LogWarning("SkeletonGraphic is null");
            return;
        }
        else
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

        // 대화가 완료되었는지 확인
        return isDialogComplete;
    }

    private async UniTask OnNextButtonClicked()
    {
        await SetNextDialogAsync();
    }

    private async UniTask SetNextDialogAsync()
    {
        nextButton.interactable = false;

        // 모든 다이얼로그 텍스트 숨기기 및 초기화
        foreach (var dialog in systemDialog)
        {
            dialog.dialogText.text = "";
            dialog.nameText.text = "";
            SetActiveObjects(dialog, false);
        }

        currentDialogIndex++;

        // 배열 범위를 벗어나는지 확인
        if (currentDialogIndex >= dialogData.Length)
        {
            // 대화 종료
            isDialogComplete = true;
            dialogCanvasGroup.gameObject.SetActive(false);
            return;
        }

        currentSpeakerIndex = dialogData[currentDialogIndex].speakerIndex;

        SetActiveObjects(systemDialog[currentSpeakerIndex], true);
        systemDialog[currentSpeakerIndex].nameText.text = dialogData[currentDialogIndex].name;

        // 타이핑 애니메이션 적용
        await TypeText(systemDialog[currentSpeakerIndex].dialogText, dialogData[currentDialogIndex].dialog);

        nextButton.interactable = true;
    }

    private async UniTask TypeText(TextMeshProUGUI textMesh, string text)
    {
        textMesh.text = ""; // 텍스트를 먼저 비웁니다.
        foreach (var t in text)
        {
            textMesh.text += t;
            await UniTask.Delay(50);
        }
    }
    
}
