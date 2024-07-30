using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NickSettingUI : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button confirmButton;
    public GameObject nickCheckUI;
    public TextMeshProUGUI nickNameText;
    public Button cancelButton;
    public Button confirmNickButton;

    public bool isComplete = false;
    public MainUI mainUI;
    private GameManager gameManager;
    
    public void Start()
    {
        gameManager = GameManager.instance;
        
        confirmButton.onClick.AddListener(OnClickConfirm);
        cancelButton.onClick.AddListener(OnClickCancel);
        confirmNickButton.onClick.AddListener(OnClickConfirmNick); 
    }
    
    private void OnClickConfirm()
    {
        gameManager.PlayerName = inputField.text;
        nickCheckUI.SetActive(true);
        nickNameText.text = gameManager.PlayerName;
        Logger.Log($"{gameManager.PlayerName}으로 설정되었습니다.");
    }

    private void OnClickCancel()
    {
        nickCheckUI.SetActive(false);
        Logger.Log($"{gameManager.PlayerName}으로 설정되었습니다.");
    }

    private void OnClickConfirmNick()
    {
        isComplete = true;
        mainUI.UpdatePlayerName();
        gameObject.SetActive(false);
        Logger.Log("닉네임 설정이 완료되었습니다.");
    }
    
    
}
