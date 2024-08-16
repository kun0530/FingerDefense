using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragInfoSlot : MonoBehaviour
{
    public TextMeshProUGUI monsterCost;
    public Button buyButton;
    public Image lockImage;
    public Image buyImage;

    private int upgradeResultId;
    private int dragLevel;

    public void SetupSlot(int cost, int dragLevel, int upgradeResultId)
    {
        this.monsterCost.text = cost.ToString();
        this.dragLevel = dragLevel;
        this.upgradeResultId = upgradeResultId;

        // 드래그 레벨에 따라 잠금 상태를 업데이트합니다.
        UpdateLockState(dragLevel == (int)GameData.MonsterDrag.LOCK);
    }

    private void Start()
    {
        UpdateCostColor();
        buyButton.onClick.AddListener(CheckBuy);
    }

    public void UpdateLockState(bool isLocked)
    {
        lockImage.gameObject.SetActive(isLocked);
        lockImage.gameObject.transform.SetAsLastSibling();
    }

    private void CheckBuy()
    {
        if (dragLevel == (int)GameData.MonsterDrag.LOCK)
        {
            ModalWindow.Create()
                .SetHeader("잠김")
                .SetBody("이 기능을 잠금 해제하려면 해당 몬스터가 등장하는 스테이지를 완료해야 합니다.")
                .AddButton("확인", () => { })
                .Show();
            return;
        }
        
        var cost = int.Parse(monsterCost.text);
        var playerGold = GameManager.instance.GameData.Gold;

        if (playerGold >= cost && dragLevel == (int)GameData.MonsterDrag.UNLOCK)
        {
            ModalWindow.Create()
                .SetHeader("구매 확인")
                .SetBody($"{cost}골드를 사용해서 해당 몬스터의 드래그 기능을 구매하시겠습니까?")
                .AddButton("확인", () =>
                {
                    GameManager.instance.GameData.Gold -= cost;
                    GameManager.instance.GameData.UpdateMonsterDragLevel(upgradeResultId, (int)GameData.MonsterDrag.ACTIVE);
                    UpdateInteractive();
                })
                .AddButton("취소", () => { })
                .Show();
        }
        else
        {
            ModalWindow.Create()
                .SetHeader("구매 실패")
                .SetBody("골드가 부족합니다.")
                .AddButton("확인", () => { })
                .Show();
        }
    }

    public void UpdateCostColor()
    {
        var cost = int.Parse(monsterCost.text);
        var playerGold = GameManager.instance.GameData.Gold;

        monsterCost.color = playerGold >= cost ? Color.black : Color.red;
    }

    private void UpdateInteractive()
    {
        dragLevel = (int)GameData.MonsterDrag.ACTIVE;
        buyButton.interactable = false;
        monsterCost.text = "구매 완료";
        monsterCost.color = Color.black;
        buyImage.gameObject.SetActive(true);
        buyImage.gameObject.transform.SetAsFirstSibling();
    }
}
