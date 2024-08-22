using UnityEngine;
using UnityEngine.UI;

public class CrystalPanel : MonoBehaviour
{
    public Button[] crystalBuyButtons;
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
        for (var i = 0; i < crystalBuyButtons.Length; i++)
        {
            var index = i;
            crystalBuyButtons[index].onClick.AddListener(() => OnClickCrystalBuyButton(index));
        }
    }

    private void OnClickCrystalBuyButton(int index)
    {
        Logger.Log($"CrystalButton {index} Clicked");
        switch (index)
        {
            case 0:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90728)
                    .AddButton("확인", () =>
                    {
                        GameManager.instance.GameData.Diamond += 60;
                        DataManager.SaveFile(GameManager.instance.GameData);
                        CheckPurchase(index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 1:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90729)
                    .AddButton("확인", () =>
                    {
                        GameManager.instance.GameData.Diamond += 330;
                        DataManager.SaveFile(GameManager.instance.GameData);
                        CheckPurchase(index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 2:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90730)
                    .AddButton("확인", () =>
                    {
                        GameManager.instance.GameData.Diamond += 1090;
                        DataManager.SaveFile(GameManager.instance.GameData);
                        CheckPurchase(index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 3:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90731)
                    .AddButton("확인", () =>
                    {
                        GameManager.instance.GameData.Diamond += 2240;
                        DataManager.SaveFile(GameManager.instance.GameData);
                        CheckPurchase(index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 4:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90732)
                    .AddButton("확인", () =>
                    {
                        GameManager.instance.GameData.Diamond += 3880;
                        DataManager.SaveFile(GameManager.instance.GameData);
                        CheckPurchase(index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            case 5:
                ModalWindow.Create()
                    .SetHeader("구매 확인")
                    .SetBody(90733)
                    .AddButton("확인", () =>
                    {
                        GameManager.instance.GameData.Diamond += 8080;
                        DataManager.SaveFile(GameManager.instance.GameData);
                        CheckPurchase(index);
                    })
                    .AddButton("취소" , () => { })
                    .Show();
                break;
            default:
                Logger.LogWarning("Invalid GoldBuyButton Clicked");
                break;
                
        }
    }
    
    private void CheckPurchase(int index)
    {
        switch (index)
        {
            case 0:
                ModalWindow.Create()
                    .SetHeader("구매 성공")
                    .SetBody(90747)
                    .AddButton("확인", () => { })
                    .Show();
                break;
            case 1:
                ModalWindow.Create()
                    .SetHeader("구매 성공")
                    .SetBody(90748)
                    .AddButton("확인", () => { })
                    .Show();
                break;
            case 2:
                ModalWindow.Create()
                    .SetHeader("구매 성공")
                    .SetBody(90749)
                    .AddButton("확인", () => { })
                    .Show();
                break;
            case 3:
                ModalWindow.Create()
                    .SetHeader("구매 성공")
                    .SetBody(90750)
                    .AddButton("확인", () => { })
                    .Show();
                break;
            case 4:
                ModalWindow.Create()
                    .SetHeader("구매 성공")
                    .SetBody(90751)
                    .AddButton("확인", () => { })
                    .Show();
                break;
            case 5:
                ModalWindow.Create()
                    .SetHeader("구매 성공")
                    .SetBody(90752)
                    .AddButton("확인", () => { })
                    .Show();
                break;
                    
                
        }
       
    }
}
