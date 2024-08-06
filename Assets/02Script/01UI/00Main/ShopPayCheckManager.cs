using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPayCheckManager : MonoBehaviour
{
    private Dictionary<int, ShopButtonType> buttonInfoDict = new Dictionary<int, ShopButtonType>();

    public Button confirmButton;
    public Button cancelButton;
    public GameObject confirmPanel; // 초기 확인 패널
    public GameObject extraConfirmPanel; // 추가 확인 팝업 패널
    public Button extraConfirmButton; // 추가 확인 창의 확인 버튼
    public Button extraCancelButton; // 추가 확인 창의 취소 버튼
    public TextMeshProUGUI resultText; // 결과 메시지를 표시할 텍스트
    public GameObject checkUIPanel;
    
    
    private ShopButtonType currentButtonType;
    private int currentButtonNumber;

    private ShopTable shopTable;
    private GameManager gameManager;

    private void Start()
    {
        shopTable = DataTableManager.Get<ShopTable>(DataTableIds.Shop);
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        extraCancelButton.onClick.AddListener(OnExtraCancelButtonClicked);
        extraConfirmButton.onClick.AddListener(OnExtraConfirmButtonClicked);
        gameManager = GameManager.instance;
    }

    public void HandleButtonClicked(ShopButtonType buttonType, int buttonNumber)
    {
        // 버튼 정보를 저장
        currentButtonType = buttonType;
        currentButtonNumber = buttonNumber;

        // 딕셔너리에 저장
        buttonInfoDict[buttonNumber] = buttonType;

        // 초기 확인 패널 활성화
        confirmPanel.SetActive(true);
    }

    private void OnConfirmButtonClicked()
    {
        // 재화 검사 로직
        bool isPurchaseSuccessful = CheckResources(currentButtonType, currentButtonNumber);

        // 확인 패널 비활성화
        confirmPanel.SetActive(false);

        if (isPurchaseSuccessful)
        {
            if (currentButtonType == ShopButtonType.Ticket)
            {
                // 티켓 구매가 성공하면 바로 컷신으로 이동
                ProcessPurchase(currentButtonType, currentButtonNumber);
                StartCutscene();
            }
            else
            {
                // 다른 타입의 경우 성공 메시지 설정 및 추가 확인 패널 활성화
                int successMessageId = GetSuccessMessageId(currentButtonType, currentButtonNumber);
                resultText.text = shopTable.Get(successMessageId);
                extraConfirmButton.gameObject.SetActive(false);
                extraConfirmPanel.SetActive(true);
            }
        }
        else
        {
            if (currentButtonType == ShopButtonType.Ticket)
            {
                if (currentButtonNumber == 1)
                {
                    if (gameManager.Ticket < 1 && gameManager.Diamond >= 160)
                    {
                        // 티켓 1개 부족하지만 다이아몬드로 구매할 수 있는 경우
                        resultText.text = "티켓이 부족하지만, 다이아몬드로 구매할 수 있습니다. 구매하시겠습니까?";
                        extraConfirmButton.gameObject.SetActive(true);
                        extraConfirmPanel.SetActive(true);
                    }
                    else if (gameManager.Ticket < 1 && gameManager.Diamond < 160)
                    {
                        // 티켓과 다이아몬드 둘 다 부족한 경우
                        resultText.text = "티켓과 다이아몬드가 부족합니다.";
                        extraConfirmButton.gameObject.SetActive(false);
                        extraConfirmPanel.SetActive(true);
                    }
                }
                else if (currentButtonNumber == 2)
                {
                    if (gameManager.Ticket < 10 && gameManager.Diamond >= 1600)
                    {
                        // 티켓 10개 부족하지만 다이아몬드로 구매할 수 있는 경우
                        resultText.text = "티켓이 부족하지만, 다이아몬드로 구매할 수 있습니다. 구매하시겠습니까?";
                        extraConfirmButton.gameObject.SetActive(true);
                        extraConfirmPanel.SetActive(true);
                    }
                    else if (gameManager.Ticket < 10 && gameManager.Diamond < 1600)
                    {
                        // 티켓과 다이아몬드 둘 다 부족한 경우
                        resultText.text = "티켓과 다이아몬드가 부족합니다.";
                        extraConfirmButton.gameObject.SetActive(false);
                        extraConfirmPanel.SetActive(true);
                    }
                }
            }
            else
            {
                // 다른 타입의 경우 실패 메시지 설정 및 추가 확인 패널 활성화
                int failMessageId = GetFailMessageId(currentButtonType, currentButtonNumber);
                resultText.text = shopTable.Get(failMessageId);
                extraConfirmButton.gameObject.SetActive(true);
                extraConfirmPanel.SetActive(true);
            }
        }
    }

    private void OnExtraConfirmButtonClicked()
    {
        // 추가 확인 창에서 확인 버튼 클릭 시 처리
        bool isPurchaseSuccessful = ConfirmAdditionalPurchase(currentButtonType, currentButtonNumber);

        if (isPurchaseSuccessful)
        {
            if (currentButtonType == ShopButtonType.Ticket)
            {
                // 티켓 구매가 성공하면 바로 컷신으로 이동
                ProcessPurchase(currentButtonType, currentButtonNumber);
                StartCutscene();
            }
            else
            {
                // 다른 타입의 경우 성공 메시지 설정
                int successMessageId = GetSuccessMessageId(currentButtonType, currentButtonNumber);
                resultText.text = shopTable.Get(successMessageId);
            }
        }
        else
        {
            // 실패 시 실패 메시지 설정
            int failMessageId = GetFailMessageId(currentButtonType, currentButtonNumber);
            resultText.text = shopTable.Get(failMessageId);
        }

        extraConfirmButton.gameObject.SetActive(false);
        extraConfirmPanel.SetActive(true);
    }

    private bool ConfirmAdditionalPurchase(ShopButtonType buttonType, int buttonNumber)
    {
        // 추가 확인 후 재화 처리 로직
        switch (buttonType)
        {
            case ShopButtonType.Ticket:
                if (buttonNumber == 1 && gameManager.Diamond >= 160)
                {
                    gameManager.Diamond -= 160;
                    gameManager.Ticket += 1;
                    return true;
                }
                else if (buttonNumber == 2 && gameManager.Diamond >= 1600)
                {
                    gameManager.Diamond -= 1600;
                    gameManager.Ticket += 10;
                    return true;
                }

                break;
            // 다른 타입의 추가 확인 로직 필요 시 추가
        }

        return false;
    }

    private void StartCutscene()
    {
        // 컷신 시작 로직
        Debug.Log("Starting cutscene...");
        // 추가 확인 패널 비활성화
        extraConfirmPanel.SetActive(false);
        checkUIPanel.SetActive(false);
    }

    private int GetSuccessMessageId(ShopButtonType buttonType, int buttonNumber)
    {
        // 각 버튼 타입과 번호에 따라 성공 메시지 ID를 반환
        switch (buttonType)
        {
            case ShopButtonType.Diamond:
                switch (buttonNumber)
                {
                    case 0:
                        return 2001; // 예시: 160 다이아 추가 메시지 ID
                    case 1:
                        return 2002; // 예시: 1600 다이아 추가 메시지 ID
                    case 2:
                        return 2003; // 예시: 4000 다이아 추가 메시지 ID
                    case 3:
                        return 2004; // 예시: 8000 다이아 추가 메시지 ID
                    case 4:
                        return 2005; // 예시: 16000 다이아 추가 메시지 ID
                    case 5:
                        return 2006; // 예시: 40000 다이아 추가 메시지 ID
                }

                break;
            case ShopButtonType.Gold:
                return 3001; // 예시: 골드 구매 성공 메시지 ID
            case ShopButtonType.Item:
                return 4001; // 예시: 아이템 구매 성공 메시지 ID
            case ShopButtonType.Mileage:
                return 5001; // 예시: 마일리지 구매 성공 메시지 ID
            default:
                return 0; // 기본 성공 메시지 ID
        }

        return 0; // 기본 성공 메시지 ID
    }

    private int GetFailMessageId(ShopButtonType buttonType, int buttonNumber)
    {
        // 각 버튼 타입과 번호에 따라 실패 메시지 ID를 반환
        switch (buttonType)
        {
            case ShopButtonType.Ticket:
                return 1003; // 예시: 티켓 구매 실패 메시지 ID
            case ShopButtonType.Diamond:
                return 2007; // 예시: 다이아 구매 실패 메시지 ID
            case ShopButtonType.Gold:
                return 3002; // 예시: 골드 구매 실패 메시지 ID
            case ShopButtonType.Item:
                return 4002; // 예시: 아이템 구매 실패 메시지 ID
            case ShopButtonType.Mileage:
                return 5002; // 예시: 마일리지 구매 실패 메시지 ID
            default:
                return 9999; // 기본 실패 메시지 ID
        }
    }

    private bool CheckResources(ShopButtonType buttonType, int buttonNumber)
    {
        switch (buttonType)
        {
            case ShopButtonType.Ticket:
                switch (buttonNumber)
                {
                    case 1:
                        if (gameManager.Ticket >= 1)
                        {
                            return true;
                        }
                        else if (gameManager.Diamond >= 160)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    case 2:
                        if (gameManager.Ticket >= 10)
                        {
                            return true;
                        }
                        else if (gameManager.Diamond >= 1600)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                }

                break;
            case ShopButtonType.Diamond:
                switch (buttonNumber)
                {
                    case 0:
                        gameManager.Diamond += 160;
                        return true;
                    case 1:
                        gameManager.Diamond += 1600;
                        return true;
                    case 2:
                        gameManager.Diamond += 4000;
                        return true;
                    case 3:
                        gameManager.Diamond += 8000;
                        return true;
                    case 4:
                        gameManager.Diamond += 16000;
                        return true;
                    case 5:
                        gameManager.Diamond += 40000;
                        return true;
                }

                break;
            case ShopButtonType.Gold:
                // 골드 구매 시 처리
                break;
            case ShopButtonType.Item:
                // 아이템 구매 시 처리
                break;
            case ShopButtonType.Mileage:
                // 마일리지 구매 시 처리
                break;
            case ShopButtonType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
        }

        return false;
    }
    private void ProcessPurchase(ShopButtonType buttonType, int buttonNumber)
    {
        switch (buttonType)
        {
            case ShopButtonType.Ticket:
                if (buttonNumber == 1 && gameManager.Ticket >= 1)
                {
                    gameManager.Ticket -= 1;
                }
                else if (buttonNumber == 2 && gameManager.Ticket >= 10)
                {
                    gameManager.Ticket -= 10;
                }
                else if (buttonNumber == 1 && gameManager.Diamond >= 160)
                {
                    gameManager.Diamond -= 160;
                    gameManager.Ticket += 1;
                }
                else if (buttonNumber == 2 && gameManager.Diamond >= 1600)
                {
                    gameManager.Diamond -= 1600;
                    gameManager.Ticket += 10;
                }
                break;
            // 다른 타입의 구매 처리 로직 필요 시 추가
        }
    }
    private void OnExtraCancelButtonClicked()
    {
        // 추가 확인 창의 취소 버튼 클릭 시 처리
        buttonInfoDict.Remove(currentButtonNumber);
        extraConfirmPanel.SetActive(false);
        checkUIPanel.SetActive(false);
        ResetPanelState();
    }

    private void OnCancelButtonClicked()
    {
        // 취소 버튼 클릭 시 처리
        // 딕셔너리에서 해당 버튼 정보 삭제
        buttonInfoDict.Remove(currentButtonNumber);

        // 추가 확인 팝업 비활성화
        extraConfirmPanel.SetActive(false);
        confirmPanel.SetActive(false);
        checkUIPanel.SetActive(false);
        ResetPanelState();
    }
    
    private void ResetPanelState()
    {
        // 패널 상태를 초기화하는 로직
        currentButtonType = ShopButtonType.None;
        currentButtonNumber = -1;
    }
}