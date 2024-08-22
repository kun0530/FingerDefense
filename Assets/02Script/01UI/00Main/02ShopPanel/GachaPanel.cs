using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaPanel : MonoBehaviour
{
    public Button[] gachaButtons;
    private GameManager gameManager;
    public GachaSystem gachaSystem; 

    public void Start()
    {
        
        gameManager = GameManager.instance;
        
        for (var i = 0; i < gachaButtons.Length; i++)
        {
            var index = i;
            
            gachaButtons[i].onClick.AddListener(() => OnClickGachaButton(index));
        }
    }

    private void OnClickGachaButton(int index)
    {
        switch (index)
        {
            case 0:
                ConfirmGachaPurchase(1, 160, index);
                break;
            case 1:
                ConfirmGachaPurchase(10, 1600, index);
                break;
        }
    }

    private void ConfirmGachaPurchase(int ticketCountRequired, int diamondCost, int index)
    {
        if (gameManager.GameData.Ticket >= ticketCountRequired)
        {
            // 티켓이 충분한 경우
            ModalWindow.Create()
                .SetHeader("구매 확인")
                .SetBody($"티켓 {ticketCountRequired}개로 {ticketCountRequired}회 뽑기를 진행하시겠습니까?")
                .AddButton("확인", () =>
                {
                    gameManager.GameData.Ticket -= ticketCountRequired;
                    gameManager.GameData.NotifyObservers(ResourceType.Ticket, gameManager.GameData.Ticket);
                    // 뽑기 진행 로직 호출 (예: 1회 또는 10회 뽑기)
                    gachaSystem.PerformGacha(ticketCountRequired);
                })
                .AddButton("취소", () => { })
                .Show();
        }
        else if (gameManager.GameData.Diamond >= diamondCost)
        {
            // 티켓이 부족하지만 다이아몬드가 충분한 경우
            int ticketsNeeded = ticketCountRequired - gameManager.GameData.Ticket;
            int diamondNeeded = ticketsNeeded * 160;

            ModalWindow.Create()
                .SetHeader("구매 확인")
                .SetBody($"티켓이 부족합니다. {ticketsNeeded}개를 {diamondNeeded} 다이아로 구매해서 {ticketCountRequired}회 뽑기를 진행하시겠습니까?")
                .AddButton("확인", () =>
                {
                    if (gameManager.GameData.Diamond >= diamondNeeded)
                    {
                        gameManager.GameData.Diamond -= diamondNeeded;
                        gameManager.GameData.NotifyObservers(ResourceType.Diamond, gameManager.GameData.Diamond);
                        // 뽑기 진행 로직 호출 (예: 1회 또는 10회 뽑기)
                        gachaSystem.PerformGacha(ticketCountRequired);
                    }
                    else
                    {
                        // 다이아몬드가 충분하지 않을 경우
                        ModalWindow.Create()
                            .SetHeader("구매 실패")
                            .SetBody("다이아몬드가 부족합니다.")
                            .AddButton("확인", () => { })
                            .Show();
                    }
                })
                .AddButton("취소", () => { })
                .Show();
        }
        else
        {
            // 티켓도 없고, 다이아몬드도 충분하지 않은 경우
            ModalWindow.Create()
                .SetHeader("구매 실패")
                .SetBody("티켓또는 다이아몬드가 부족합니다.")
                .AddButton("확인", () => { })
                .Show();
        }
    }
}
