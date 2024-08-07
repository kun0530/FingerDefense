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
    public GameObject confirmPanel;
    public GameObject extraConfirmPanel;
    public Button extraConfirmButton;
    public Button extraCancelButton;
    public TextMeshProUGUI resultText;
    public GameObject checkUIPanel;

    private ShopButtonType currentButtonType;
    private int currentButtonNumber;
    private ShopTable shopTable;
    private GameManager gameManager;
    public GachaSystem gachaSystem;

    private void Start()
    {
        shopTable = DataTableManager.Get<ShopTable>(DataTableIds.Shop);
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        extraCancelButton.onClick.AddListener(OnExtraCancelButtonClicked);
        extraConfirmButton.onClick.AddListener(OnExtraConfirmButtonClicked);

        gameManager = GameManager.instance;
        if (gachaSystem == null)
        {
            Debug.LogError("GachaSystem이 참조되지 않았습니다!");
        }
        else
        {
            Debug.Log("GachaSystem이 성공적으로 참조되었습니다.");
        }
    }

    public void HandleButtonClicked(ShopButtonType buttonType, int buttonNumber)
    {
        currentButtonType = buttonType;
        currentButtonNumber = buttonNumber;
        buttonInfoDict[buttonNumber] = buttonType;
        confirmPanel.SetActive(true);
    }

    private void OnConfirmButtonClicked()
    {
        confirmPanel.SetActive(false);
        if (CheckResources(currentButtonType, currentButtonNumber))
        {
            HandleSuccessfulPurchase();
        }
        else
        {
            HandleFailedPurchase();
        }
    }

    private void OnExtraConfirmButtonClicked()
    {
        var isPurchaseSuccessful = ConfirmAdditionalPurchase(currentButtonType, currentButtonNumber);
        UpdateResultText(isPurchaseSuccessful);

        if (isPurchaseSuccessful)
        {
            ProcessPurchase(currentButtonType, currentButtonNumber);
            if (currentButtonType == ShopButtonType.Ticket)
            {
                StartCutscene();
                if (!gachaSystem.gameObject.activeInHierarchy)
                {
                    gachaSystem.gameObject.SetActive(true);
                }
                gachaSystem.PerformGacha(currentButtonNumber == 1 ? 1 : 10);
                Logger.Log("GachaSystem.PerformGacha 호출됨");
            }
        }

        extraConfirmButton.gameObject.SetActive(false);
        extraConfirmPanel.SetActive(false);
        checkUIPanel.SetActive(false);
    }

    private void OnExtraCancelButtonClicked()
    {
        ResetState();
    }

    private void OnCancelButtonClicked()
    {
        ResetState();
    }

    private void HandleSuccessfulPurchase()
    {
        if (currentButtonType == ShopButtonType.Ticket)
        {
            ProcessPurchase(currentButtonType, currentButtonNumber);
            
            if (!gachaSystem.gameObject.activeInHierarchy)
            {
                gachaSystem.gameObject.SetActive(true);
            }
            
            gachaSystem.PerformGacha(currentButtonNumber == 1 ? 1 : 10);
            Logger.Log("GachaSystem.PerformGacha 호출됨");
            StartCutscene();
        }
        else
        {
            var successMessageId = GetSuccessMessageId(currentButtonType, currentButtonNumber);
            resultText.text = shopTable.Get(successMessageId);
            extraConfirmButton.gameObject.SetActive(false);
            extraConfirmPanel.SetActive(true);
        }
    }

    private void HandleFailedPurchase()
    {
        if (currentButtonType == ShopButtonType.Ticket)
        {
            HandleTicketFailure();
        }
        else
        {
            var failMessageId = GetFailMessageId(currentButtonType, currentButtonNumber);
            resultText.text = shopTable.Get(failMessageId);
            extraConfirmButton.gameObject.SetActive(true);
            extraConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "확인";
            extraConfirmPanel.SetActive(true);
        }
    }

    private void HandleTicketFailure()
    {
        var message = GetTicketFailureMessage(currentButtonNumber, out var showConfirmButton);

        resultText.text = message;
        extraConfirmButton.gameObject.SetActive(showConfirmButton);
        extraConfirmPanel.SetActive(true);
    }

    private string GetTicketFailureMessage(int buttonNumber, out bool showConfirmButton)
    {
        var requiredTickets = buttonNumber == 1 ? 1 : 10;
        var ticketsNeeded = requiredTickets - gameManager.Ticket;
        var diamondsNeeded = ticketsNeeded * 160;

        if (gameManager.Ticket < requiredTickets && gameManager.Diamond >= diamondsNeeded)
        {
            showConfirmButton = true;
            return $"캐릭터 모집 티켓의 개수가 부족합니다. {ticketsNeeded}개 만큼 {diamondsNeeded} 다이아로 구매해서 사용하시겠습니까?";
        }
        if (gameManager.Ticket < requiredTickets && gameManager.Diamond < diamondsNeeded)
        {
            showConfirmButton = false;
            extraCancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "확인";
            return "티켓이 부족합니다.";
        }
        else
        {
            showConfirmButton = false;
            return "티켓이 부족합니다.";
        }
    }

    private void UpdateResultText(bool isPurchaseSuccessful)
    {
        var messageId = isPurchaseSuccessful
            ? GetSuccessMessageId(currentButtonType, currentButtonNumber)
            : GetFailMessageId(currentButtonType, currentButtonNumber);
        resultText.text = shopTable.Get(messageId);
    }

    private void StartCutscene()
    {
        Debug.Log("Starting cutscene...");
        checkUIPanel.SetActive(false);
        extraConfirmPanel.SetActive(false);

        // if (gachaSystem)
        // {
        //     
        // }
    }

    #region MessageId
    private int GetSuccessMessageId(ShopButtonType buttonType, int buttonNumber)
    {
        return buttonType switch
        {
            ShopButtonType.Diamond => buttonNumber switch
            {
                0 => 2001,
                1 => 2002,
                2 => 2003,
                3 => 2004,
                4 => 2005,
                5 => 2006,
                _ => 0,
            },
            ShopButtonType.Gold => buttonNumber switch
            {
                0 => 2001,
                1 => 2002,
                2 => 2003,
                3 => 2004,
                4 => 2005,
                5 => 2006,
                _ => 0,
            },
            ShopButtonType.Item => buttonNumber switch
            {
                0 => 2001,
                1 => 2002,
                2 => 2003,
                3 => 2004,
                4 => 2005,
                5 => 2006,
                6 => 2007,
                7 => 2008,
                8 => 2009,
                9 => 2010,
                10 => 2011,
                _ => 0,
            },
            ShopButtonType.Mileage => buttonNumber switch
            {
                0 => 2001,
                1 => 2002,
                2 => 2003,
                3 => 2004,
                4 => 2005,
                5 => 2006,
                6 => 2007,
                7 => 2008,
                8 => 2009,
                9 => 2010,
                10 => 2011,
                _ => 0,
            },
            _ => 0,
        };
    }

    private int GetFailMessageId(ShopButtonType buttonType, int buttonNumber)
    {
        return buttonType switch
        {
            ShopButtonType.Ticket => 1003,
            ShopButtonType.Diamond => buttonNumber switch
            {
                0 => 3001,
                1 => 3002,
                2 => 3003,
                3 => 3004,
                4 => 3005,
                5 => 3006,
                _ => 9999,
            },
            ShopButtonType.Gold => buttonNumber switch
            {
                0 => 3001,
                1 => 3002,
                2 => 3003,
                3 => 3004,
                4 => 3005,
                5 => 3006,
                _ => 9999,
            },
            ShopButtonType.Item => buttonNumber switch
            {
                0 => 3001,
                1 => 3002,
                2 => 3003,
                3 => 3004,
                4 => 3005,
                5 => 3006,
                6 => 3007,
                7 => 3008,
                8 => 3009,
                9 => 3010,
                10 => 3011,
                _ => 9999,
            },
            ShopButtonType.Mileage => buttonNumber switch
            {
                0 => 3001,
                1 => 3002,
                2 => 3003,
                3 => 3004,
                4 => 3005,
                5 => 3006,
                6 => 3007,
                7 => 3008,
                8 => 3009,
                9 => 3010,
                10 => 3011,
                _ => 9999,
            },
            _ => 9999,
        };
    }
    #endregion

    private bool CheckResources(ShopButtonType buttonType, int buttonNumber)
    {
        if (buttonType == ShopButtonType.Ticket)
        {
            return CheckTicketResources(buttonNumber, out bool needsExtraConfirmation);
        }
        return buttonType switch
        {
            ShopButtonType.Diamond => AddDiamonds(buttonNumber),
            _ => false,
        };
    }

    private bool CheckTicketResources(int buttonNumber, out bool needsExtraConfirmation)
    {
        needsExtraConfirmation = false;
        switch (buttonNumber)
        {
            case 1 when gameManager.Ticket >= 1:
                return true;
            case 1 when gameManager.Diamond >= 160:
                needsExtraConfirmation = true;
                return false;
            case 2 when gameManager.Ticket >= 10:
                return true;
            case 2 when gameManager.Ticket >= 1 && gameManager.Diamond >= (10 - gameManager.Ticket) * 160:
                needsExtraConfirmation = true;
                return false;
            default:
                return false;
        }
    }

    private bool AddDiamonds(int buttonNumber)
    {
        var diamondsToAdd = buttonNumber switch
        {
            0 => 160,
            1 => 1600,
            2 => 4000,
            3 => 8000,
            4 => 16000,
            5 => 40000,
            _ => 0,
        };
        gameManager.Diamond += diamondsToAdd;
        return true;
    }

    private bool ConfirmAdditionalPurchase(ShopButtonType buttonType, int buttonNumber)
    {
        if (buttonType == ShopButtonType.Ticket)
        {
            switch (buttonNumber)
            {
                case 1 when gameManager.Ticket < 1 && gameManager.Diamond >= 160:
                case 2 when gameManager.Ticket < 10 && gameManager.Diamond >= (10 - gameManager.Ticket) * 160:
                    return true;
            }
        }
        return false;
    }

    private void ProcessPurchase(ShopButtonType buttonType, int buttonNumber)
    {
        switch (buttonType)
        {
            case ShopButtonType.Ticket:
                if (buttonNumber == 1)
                {
                    if (gameManager.Ticket >= 1)
                    {
                        gameManager.RemoveTickets(1);
                    }
                    else if (gameManager.Diamond >= 160)
                    {
                        gameManager.RemoveDiamonds(160);
                        gameManager.AddTickets(1);
                    }
                }
                else if (buttonNumber == 2)
                {
                    int requiredTickets = 10;
                    int currentTickets = gameManager.Ticket;
                    int ticketsToBuyWithDiamonds = requiredTickets - currentTickets;
                    int diamondCost = ticketsToBuyWithDiamonds * 160;

                    if (gameManager.Ticket >= 10)
                    {
                        gameManager.RemoveTickets(10);
                    }
                    else if (gameManager.Ticket >= 1 && gameManager.Diamond >= diamondCost)
                    {
                        gameManager.RemoveTickets(currentTickets);
                        gameManager.RemoveDiamonds(diamondCost);
                        gameManager.AddTickets(10 - currentTickets);
                    }
                    else if (gameManager.Diamond >= diamondCost)
                    {
                        gameManager.RemoveDiamonds(diamondCost);
                        gameManager.AddTickets(10);
                    }
                }
                break;
            case ShopButtonType.Diamond:
                int diamondsToAdd = buttonNumber switch
                {
                    0 => 160,
                    1 => 1600,
                    2 => 4000,
                    3 => 8000,
                    4 => 16000,
                    5 => 40000,
                    _ => 0,
                };
                gameManager.Diamond += diamondsToAdd;
                break;
            // 다른 타입의 구매 처리 로직 필요 시 추가
        }
    }

    private void ResetState()
    {
        buttonInfoDict.Remove(currentButtonNumber);
        extraConfirmPanel.SetActive(false);
        confirmPanel.SetActive(false);
        checkUIPanel.SetActive(false);
        ResetPanelState();
    }

    private void ResetPanelState()
    {
        currentButtonType = ShopButtonType.None;
        currentButtonNumber = -1;
    }
}
