using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using DG.Tweening;

public class NickSettingUI : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button confirmButton;
    public GameObject nickCheckUI;
    public TextMeshProUGUI nickNameText;
    public Button cancelButton;
    public Button confirmNickButton;

    [HideInInspector]
    public bool isComplete = false;
    public MainUI mainUI;
    private GameManager gameManager;
    private readonly Regex koreanFullSyllablesRegex = new("^[가-힣a-zA-Z0-9]*$");
    private string userId;

    public TextMeshProUGUI noticeText;
    private Vector3 noticeTextInitialPosition;
    
    public void Start()
    {
        gameManager = GameManager.instance;
        
        
        
        confirmButton.onClick.AddListener(OnClickConfirm);
        cancelButton.onClick.AddListener(OnClickCancel);
        confirmNickButton.onClick.AddListener(OnClickConfirmNick);
        
        noticeTextInitialPosition = noticeText.transform.localPosition;
    }
    
    private void OnClickConfirm()
    {
        userId = inputField.text;
        if(userId.Length is < 4 or > 10)
        {
            ShowNotice("닉네임은 4자 이상 10자 이하로 입력해주세요.");
            Logger.Log("닉네임은 4자 이상 10자 이하로 입력해주세요.");
            return;
        }
        if(!koreanFullSyllablesRegex.IsMatch(userId))
        {
            ShowNotice("닉네임은 한글 음절, 영어, 숫자만 입력 가능합니다.");
            Logger.Log("닉네임은 한글 음절, 영어, 숫자만 입력 가능합니다.");
            return;
        }
        
        gameManager.PlayerName = inputField.text;
        Variables.LoadName.Nickname = gameManager.PlayerName;
        nickCheckUI.SetActive(true);
        nickNameText.text = Variables.LoadName.Nickname;
        
        mainUI.UpdatePlayerName();
        
        Logger.Log($"{gameManager.PlayerName}으로 설정되었습니다.");
    }

    private void OnClickCancel()
    {
        nickCheckUI.SetActive(false);
        Variables.LoadName.Nickname = "";
        Logger.Log($"{gameManager.PlayerName}으로 설정되었습니다.");
    }

    private void OnClickConfirmNick()
    {
        isComplete = true;
        
        if(mainUI == null)
        {
            mainUI=GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>();
            return;
        }
        
        mainUI.UpdatePlayerName();
        gameObject.SetActive(false);
        Logger.Log("닉네임 설정이 완료되었습니다.");
    }
    
    private void ShowNotice(string message)
    {
        noticeText.text = message;
        noticeText.color = new Color(noticeText.color.r, noticeText.color.g, noticeText.color.b, 1);
        noticeText.transform.localPosition = noticeTextInitialPosition;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.5f);
        sequence.Append(noticeText.DOFade(0, 1f));
        sequence.Join(noticeText.transform.DOLocalMoveY(noticeTextInitialPosition.y + 50, 1f).SetEase(Ease.InQuad));
        sequence.Play();
    }
}
