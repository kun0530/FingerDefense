using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldPanel : MonoBehaviour
{
    public Button[] GoldBuyButtons;
    private StringTable stringTable;
    
    private void Awake()
    {
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
    }
    
    private void Start()
    {
        AssignButtonEvents();
    }

    private void AssignButtonEvents()
    {
        for (var i = 0; i < GoldBuyButtons.Length; i++)
        {
            var index = i;
            GoldBuyButtons[index].onClick.AddListener(() => OnClickGoldBuyButton(index));
        }
    }

    private void OnClickGoldBuyButton(int index)
    {
        Logger.Log($"GoldBuyButton {index} Clicked");
        switch (index)
        {
            case 0:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90741)
                    .AddButton("확인", () =>
                    {
                        CheckPurchase(80, 1600, index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 1:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90742)
                    .AddButton("확인", () =>
                    {
                        CheckPurchase(200,4000, index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 2:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90743)
                    .AddButton("확인", () =>
                    {
                        CheckPurchase(500,10000, index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 3:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90744)
                    .AddButton("확인", () =>
                    {
                        CheckPurchase(1000,20000, index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 4:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90745)
                    .AddButton("확인", () =>
                    {
                        CheckPurchase(3000,60000, index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 5:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90746)
                    .AddButton("확인", () =>
                    {
                        CheckPurchase(5000, 100000, index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            default:
                Logger.LogWarning("Invalid GoldBuyButton Clicked");
                break;
                
        }
    }
    
    private void CheckPurchase(int diamond, int gold,int index)
    {
        if (GameManager.instance.GameData.Diamond >= diamond)
        {
            switch (index)
            {
                case 0:
                    ModalWindow.Create()
                        .SetHeader("구매 성공")
                        .SetBody(90747)
                        .AddButton("확인", () =>
                        {
                            GameManager.instance.GameData.Diamond -= diamond;
                            GameManager.instance.GameData.Gold += gold;
                            DataManager.SaveFile(GameManager.instance.GameData);
                        })
                        .Show();
                    break;
                case 1:
                    ModalWindow.Create()
                        .SetHeader("구매 성공")
                        .SetBody(90748)
                        .AddButton("확인", () =>
                        {
                            GameManager.instance.GameData.Diamond -= diamond;
                            GameManager.instance.GameData.Gold += gold;
                            DataManager.SaveFile(GameManager.instance.GameData);
                        })
                        .Show();
                    break;
                case 2:
                    ModalWindow.Create()
                        .SetHeader("구매 성공")
                        .SetBody(90749)
                        .AddButton("확인", () =>
                        {
                            GameManager.instance.GameData.Diamond -= diamond;
                            GameManager.instance.GameData.Gold += gold;
                            DataManager.SaveFile(GameManager.instance.GameData);
                        })
                        .Show();
                    break;
                case 3:
                    ModalWindow.Create()
                        .SetHeader("구매 성공")
                        .SetBody(90750)
                        .AddButton("확인", () =>
                        {
                            GameManager.instance.GameData.Diamond -= diamond;
                            GameManager.instance.GameData.Gold += gold;
                            DataManager.SaveFile(GameManager.instance.GameData);
                        })
                        .Show();
                    break;
                case 4:
                    ModalWindow.Create()
                        .SetHeader("구매 성공")
                        .SetBody(90751)
                        .AddButton("확인", () =>
                        {
                            GameManager.instance.GameData.Diamond -= diamond;
                            GameManager.instance.GameData.Gold += gold;
                            DataManager.SaveFile(GameManager.instance.GameData);
                        })
                        .Show();
                    break;
                case 5:
                    ModalWindow.Create()
                        .SetHeader("구매 성공")
                        .SetBody(90752)
                        .AddButton("확인", () =>
                        {
                            GameManager.instance.GameData.Diamond -= diamond;
                            GameManager.instance.GameData.Gold += gold;
                            DataManager.SaveFile(GameManager.instance.GameData);
                        })
                        .Show();
                    break;
                    
            }    
        }
        else
        {
            ModalWindow.Create()
                .SetHeader("구매 실패")
                .SetBody(90753)
                .AddButton("확인", () => { })
                .Show();
        }
    }
}
