using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public void Start()
    {
        confirmButton.onClick.AddListener(OnClickConfirm);
        cancelButton.onClick.AddListener(OnClickCancel);
        confirmNickButton.onClick.AddListener(OnClickConfirmNick); 
    }
    
    private void OnClickConfirm()
    {
        Variables.PlayerName.playerName = inputField.text;
        nickCheckUI.SetActive(true);
        nickNameText.text = Variables.PlayerName.playerName;
        Logger.Log($"{Variables.PlayerName.playerName}으로 설정되었습니다.");
    }

    private void OnClickCancel()
    {
        nickCheckUI.SetActive(false);
        Variables.PlayerName.playerName = "";
        Logger.Log($"{Variables.PlayerName.playerName}으로 설정되었습니다.");
    }

    private void OnClickConfirmNick()
    {
        isComplete = true;
        gameObject.SetActive(false);
        Logger.Log("닉네임 설정이 완료되었습니다.");
    }
    
    
}
