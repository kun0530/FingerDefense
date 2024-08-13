using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DragInfoSlot : MonoBehaviour
{
    public TextMeshProUGUI monsterCost;
    public Button buyButton;
    
    private void Start()
    {
        UpdateCostColor();
    }

    public void UpdateCostColor()
    {
        var cost = int.Parse(monsterCost.text);
        var playerGold = GameManager.instance.ResourceManager.Gold;

        monsterCost.color = playerGold >= cost ? Color.black : // 구매 가능 시 검은색
            Color.red; // 구매 불가능 시 빨간색
    }
}
