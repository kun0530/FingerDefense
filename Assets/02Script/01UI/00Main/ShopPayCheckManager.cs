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
    private StringTable shopTable;
    private GameManager gameManager;
    public GachaSystem gachaSystem;

    private void Start()
    {
        shopTable = DataTableManager.Get<StringTable>(DataTableIds.String);
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        extraCancelButton.onClick.AddListener(OnExtraCancelButtonClicked);
        extraConfirmButton.onClick.AddListener(OnExtraConfirmButtonClicked);

        gameManager = GameManager.instance;
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
                EnsureGachaSystemActive();
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
            EnsureGachaSystemActive();
            StartCutscene();
        }
        else if(currentButtonType == ShopButtonType.Gold)
        {
            ProcessPurchase(currentButtonType, currentButtonNumber);
            extraConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "확인";
            extraConfirmButton.gameObject.SetActive(false);
            extraConfirmPanel.SetActive(true);
        }
        else
        {
            var successMessageId = GetSuccessMessageId(currentButtonType, currentButtonNumber);
            resultText.text = shopTable.Get(successMessageId.ToString());
            extraConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "확인";
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
        else if (currentButtonType == ShopButtonType.Gold)
        {
            ShowPurchaseResult("다이아가 부족하여 골드를 구매할 수 없습니다.");
            extraCancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "확인";
        }
        else
        {
            var failMessageId = GetFailMessageId(currentButtonType, currentButtonNumber);
            resultText.text = shopTable.Get(failMessageId.ToString());
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
        resultText.text = shopTable.Get(messageId.ToString());
    }

    private void StartCutscene()
    {
        Debug.Log("Starting cutscene...");
        checkUIPanel.SetActive(false);
        extraConfirmPanel.SetActive(false);
    }

    #region MessageId
    private int GetSuccessMessageId(ShopButtonType buttonType, int buttonNumber)
    {
        return buttonType switch
        {
            ShopButtonType.Diamond => buttonNumber switch
            {
                0 => 90734,
                1 => 90735,
                2 => 90736,
                3 => 90737,
                4 => 90738,
                5 => 90739,
                _ => 0,
            },
            ShopButtonType.Gold => buttonNumber switch
            {
                0 => 90747,
                1 => 90748,
                2 => 90749,
                3 => 90750,
                4 => 90751,
                5 => 90752,
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
            ShopButtonType.Diamond => buttonNumber switch
            {
                0 => 90740,
                1 => 90740,
                2 => 90740,
                3 => 90740,
                4 => 90740,
                5 => 90740,
                _ => 90740,
            },
            ShopButtonType.Gold => buttonNumber switch
            {
                0 => 90753,
                1 => 90753,
                2 => 90753,
                3 => 90753,
                4 => 90753,
                5 => 90753,
                _ => 90753,
            },
            ShopButtonType.Item => buttonNumber switch
            {
                0 => 90768,
                1 => 90768,
                2 => 90768,
                3 => 90768,
                4 => 90768,
                5 => 90768,
                6 => 90768,
                7 => 90768,
                8 => 90768,
                9 => 90768,
                10 => 90768,
                _ => 90768,
            },
            ShopButtonType.Mileage => buttonNumber switch
            {
                0 => 90817,
                1 => 90817,
                2 => 90817,
                3 => 90817,
                4 => 90817,
                5 => 90817,
                6 => 90817,
                7 => 90817,
                8 => 90817,
                9 => 90817,
                10 => 90817,
                _ => 90817,
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

        if (buttonType == ShopButtonType.Gold)
        {
            return CheckGoldResources(buttonNumber);
        }
        return buttonType switch
        {
            ShopButtonType.Diamond => AddDiamonds(buttonNumber),
            _ => false,
        };
    }

    private bool CheckGoldResources(int buttonNumber)
    {
        var diamondCostForGold = buttonNumber switch
        {
            0 => 80,
            1 => 200,
            2 => 500,
            3 => 1000,
            4 => 3000,
            5 => 5000,
            _ => 0,
        };
        return gameManager.Diamond >= diamondCostForGold;
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
            0 => 60,
            1 => 330,
            2 => 1090,
            3 => 2240,
            4 => 3880,
            5 => 8080,
            _ => 0,
        };
        gameManager.Diamond += diamondsToAdd;
        extraCancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "확인";
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
        int diamondCost;
        switch (buttonType)
        {
            case ShopButtonType.Ticket:
                if (buttonNumber == 1)
                {
                    if (gameManager.Ticket >= 1)
                    {
                        gameManager.RemoveTickets(1);
                        gachaSystem.PerformGacha(1);
                    }
                    else if (gameManager.Diamond >= 160 && gameManager.Ticket == 0)
                    {
                        gameManager.RemoveDiamonds(160);
                        gachaSystem.PerformGacha(1);
                    }
                }
                else if (buttonNumber == 2)
                {
                    if (gameManager.Ticket is 10 or > 10)
                    {
                        gameManager.RemoveTickets(10);
                        gachaSystem.PerformGacha(10);    
                    }
                    else
                    {
                        int ticketsNeeded = 10 - gameManager.Ticket;
                        diamondCost = ticketsNeeded * 160;

                        if (gameManager.Diamond >= diamondCost)
                        {
                            if (gameManager.Ticket > 0)
                            {
                                gameManager.RemoveTickets(gameManager.Ticket);
                            }
                            gameManager.RemoveDiamonds(diamondCost);
                            gachaSystem.PerformGacha(10);
                        }
                    }
                }
                break;
            case ShopButtonType.Diamond:
                var diamondsToAdd = buttonNumber switch
                {
                    0 => 60,
                    1 => 330,
                    2 => 1090,
                    3 => 2240,
                    4 => 3880,
                    5 => 8080,
                    _ => 0,
                };
                gameManager.Diamond += diamondsToAdd;
                ShowPurchaseResult($"{diamondsToAdd} 다이아를 구매했습니다.");
                break;
            
            case ShopButtonType.Gold:
                diamondCost = buttonNumber switch
                {
                    0 => 80,
                    1 => 200,
                    2 => 500,
                    3 => 1000,
                    4 => 3000,
                    5 => 5000,
                    _ => 0,
                };
                var goldAmount = buttonNumber switch
                {
                    0 => 1600,
                    1 => 4000,
                    2 => 10000,
                    3 => 20000,
                    4 => 60000,
                    5 => 100000,
                    _ => 0,
                };

                if (gameManager.Diamond >= diamondCost)
                {
                    gameManager.RemoveDiamonds(diamondCost); // 다이아 차감
                    gameManager.AddGold(goldAmount); // 골드 추가
                    
                    var messageId = GetSuccessMessageId(buttonType, buttonNumber);
                    var successMessage = shopTable.Get(messageId.ToString());
                
                    ShowPurchaseResult(successMessage);
                }
                else
                {
                    var failMessageId = GetFailMessageId(buttonType, buttonNumber);
                    var failMessage = shopTable.Get(failMessageId.ToString());
                    ShowPurchaseResult(failMessage);
                }
                break;
            // 다른 타입의 구매 처리 로직 필요 시 추가
        }
    }

    private void ShowPurchaseResult(string message)
    {
        resultText.text = message;
        extraConfirmButton.gameObject.SetActive(false);
        extraCancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "확인";
        extraConfirmPanel.SetActive(true);
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
    
    private void EnsureGachaSystemActive()
    {
        if (!gachaSystem.gameObject.activeInHierarchy)
        {
            gachaSystem.gameObject.SetActive(true);
            gachaSystem.transform.SetAsLastSibling();
        }
    }
}
