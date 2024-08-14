using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DragInfoSlot : MonoBehaviour
{
    public TextMeshProUGUI monsterCost;
    public Button buyButton;
    
    private StringTable stringTable;
    
    private void Awake()
    {
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
    }
    
    private void Start()
    {
        UpdateCostColor();
        buyButton.onClick.AddListener(CheckBuy);
    }

    private void CheckBuy()
    {
        var cost = int.Parse(monsterCost.text);
        var playerGold = GameManager.instance.GameData.Gold;

        if (playerGold >= cost)
        {
            ModalWindow.Create()
                .SetHeader("구매 확인")
                .SetBody($"{cost}골드를 사용해서 해당 몬스터의 드래그기능을 구매하시겠습니까?")
                .AddButton("확인", () =>
                {
                    GameManager.instance.GameData.Gold -= cost;
                    UpdateInteractive();
                })
                .AddButton("취소", () =>
                {
                })
                .Show();
        }
        else
        {
            ModalWindow.Create()
                .SetHeader("구매 실패")
                .SetBody("골드가 부족합니다.")
                .AddButton("확인", () =>
                {
                })
                .Show();
        }
    }

    public void UpdateCostColor()
    {
        var cost = int.Parse(monsterCost.text);
        var playerGold = GameManager.instance.GameData.Gold;

        monsterCost.color = playerGold >= cost ? Color.black : // 구매 가능 시 검은색
            Color.red; // 구매 불가능 시 빨간색
    }

    private void UpdateInteractive()
    {
        buyButton.interactable = false;
        monsterCost.text = "구매 완료";
        monsterCost.color = Color.black;
        
        // 구매 완료 시 해당 아이디랑 같은 몬스터의 드래그 기능 활성화
        
    }
}
