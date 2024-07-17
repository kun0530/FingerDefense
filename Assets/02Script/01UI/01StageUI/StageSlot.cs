using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    public TextMeshProUGUI stageNameText;

    public RectTransform monsterSlotParent;
    public RectTransform rewardSlotParent;

    public GameObject monsterSlotPrefab; 
    public GameObject rewardSlotPrefab;  

    public void Configure(StageData stageData)
    {
        stageNameText.text = stageData.StageNameId.ToString();

        if (stageData.Monster1Id != 0) AddMonsterSlot(stageData.Monster1Id);
        if (stageData.Monster2Id != 0) AddMonsterSlot(stageData.Monster2Id);
        if (stageData.Monster3Id != 0) AddMonsterSlot(stageData.Monster3Id);

        if (stageData.Reward1Id != 0 && stageData.Reward1Value != 0) AddRewardSlot(stageData.Reward1Id, stageData.Reward1Value);
        if (stageData.Reward2Id != 0 && stageData.Reward2Value != 0) AddRewardSlot(stageData.Reward2Id, stageData.Reward2Value);
    }

    private void AddMonsterSlot(int monsterId)
    {
        var monsterSlot = Instantiate(monsterSlotPrefab, monsterSlotParent);
        var monsterImage = monsterSlot.GetComponentInChildren<Image>();
        var monsterText = monsterSlot.GetComponentInChildren<TextMeshProUGUI>();
        
        // To-Do 데이터 테이블로 불러올 수 있도록 수정 예정
        var monsterSprite = Resources.Load<Sprite>($"Monsters/{monsterId}") ? Resources.Load<Sprite>($"Monsters/{monsterId}") : Resources.Load<Sprite>("PlaceholderImage");
        if (monsterSprite != null)
        {
            monsterImage.sprite = monsterSprite;
        }
        else
        {
            Debug.LogWarning($"Monster image not found for ID: {monsterId}");
        }

        monsterText.text = monsterId.ToString();
    }

    private void AddRewardSlot(int rewardId, int rewardValue)
    {
        GameObject rewardSlot = Instantiate(rewardSlotPrefab, rewardSlotParent);
        Image rewardImage = rewardSlot.GetComponentInChildren<Image>();
        TextMeshProUGUI rewardText = rewardSlot.GetComponentInChildren<TextMeshProUGUI>();

        // To-Do 데이터 테이블로 불러올 수 있도록 수정 예정
        Sprite rewardSprite = Resources.Load<Sprite>($"Rewards/{rewardId}") ? Resources.Load<Sprite>($"Rewards/{rewardId}") : Resources.Load<Sprite>("PlaceholderImage");
        if (rewardSprite != null)
        {
            rewardImage.sprite = rewardSprite;
        }
        else
        {
            Debug.LogWarning($"Reward image not found for ID: {rewardId}");
        }

        rewardText.text = $"{rewardValue}";
    }
}
