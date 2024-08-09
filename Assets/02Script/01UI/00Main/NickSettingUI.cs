using System;
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

    public void Awake()
    {
        gameManager = GameObject.FindWithTag("Manager").TryGetComponent(out GameManager manager) ? manager : null;    
    }

    public void Start()
    {
        confirmButton.onClick.AddListener(OnClickConfirm);
        cancelButton.onClick.AddListener(OnClickCancel);
        confirmNickButton.onClick.AddListener(OnClickConfirmNick);
        
        noticeTextInitialPosition = noticeText.transform.localPosition;
        
        inputField.onSelect.AddListener(OnInputFieldSelected);
    }
    private void OnInputFieldSelected(string text)
    {
        inputField.ActivateInputField();
    }
    private void OnClickConfirm()
    {
        userId = inputField.text;
        if(userId.Length is < 2 or > 8)
        {
            ShowNotice("닉네임은 2자 이상 8자 이하로 입력해주세요.");
            Logger.Log("닉네임은 2자 이상 8자 이하로 입력해주세요.");
            return;
        }
        if(!koreanFullSyllablesRegex.IsMatch(userId))
        {
            ShowNotice("닉네임은 한글 음절, 영어, 숫자만 입력 가능합니다.");
            Logger.Log("닉네임은 한글 음절, 영어, 숫자만 입력 가능합니다.");
            return;
        }
        
        gameManager.PlayerName = inputField.text;
        
        nickCheckUI.SetActive(true);
        nickNameText.text = $"정말 <color=#FF0000>{gameManager.PlayerName}</color>으로 설정하시겠습니까?";
        
        Logger.Log($"{gameManager.PlayerName}으로 설정되었습니다.");
    }

    private void OnClickCancel()
    {
        nickCheckUI.SetActive(false);
        gameManager.PlayerName = "";
        Logger.Log($"{gameManager.PlayerName}으로 설정되었습니다.");
    }

    private void OnClickConfirmNick()
    {
        isComplete = true;
        mainUI.UpdatePlayerInfo();
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
