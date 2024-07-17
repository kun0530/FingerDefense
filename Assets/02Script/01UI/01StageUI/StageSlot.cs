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

        AddMonsterSlot(stageData.Monster1Id);
        AddMonsterSlot(stageData.Monster2Id);
        AddMonsterSlot(stageData.Monster3Id);

        AddRewardSlot(stageData.Reward1Id, stageData.Reward1Value);
        AddRewardSlot(stageData.Reward2Id, stageData.Reward2Value);
    }

    private void AddMonsterSlot(int monsterId)
    {
        GameObject monsterSlot = Instantiate(monsterSlotPrefab, monsterSlotParent);
        Image monsterImage = monsterSlot.GetComponentInChildren<Image>();
        TextMeshProUGUI monsterText = monsterSlot.GetComponentInChildren<TextMeshProUGUI>();

        // 리소스에서 몬스터 이미지 로드 또는 플레이스홀더 이미지 사용

        monsterText.text = monsterId.ToString();
    }

    private void AddRewardSlot(int rewardId, int rewardValue)
    {
        GameObject rewardSlot = Instantiate(rewardSlotPrefab, rewardSlotParent);
        Image rewardImage = rewardSlot.GetComponentInChildren<Image>();
        TextMeshProUGUI rewardText = rewardSlot.GetComponentInChildren<TextMeshProUGUI>();

        // 리소스에서 보상 이미지 로드 또는 플레이스홀더 이미지 사용
        
        rewardText.text = $"{rewardValue}";
    }
}