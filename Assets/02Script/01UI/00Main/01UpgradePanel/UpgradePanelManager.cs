using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelManager : MonoBehaviour
{
    public MonsterDragPanel monsterUpgradePanelPrefab;
    public MonsterGimmickPanel monsterGimmickPanelPrefab;
    public CharacterUpgradePanel characterUpgradePanelPrefab;
    public CharacterFeaturePanel characterGimmickPanelPrefab;
    
    public Button[] upgradePanelButtons; // 버튼 배열 (0: 몬스터 업그레이드, 1: 몬스터 기믹, 2: 캐릭터 업그레이드, 3: 캐릭터 기믹)
    public TextMeshProUGUI[] costTexts; //재화 관련
    
    private void Start()
    {
        upgradePanelButtons[0].onClick.AddListener(() => ShowPanel(monsterUpgradePanelPrefab));
        upgradePanelButtons[1].onClick.AddListener(() => ShowPanel(monsterGimmickPanelPrefab));
        upgradePanelButtons[2].onClick.AddListener(() => ShowPanel(characterUpgradePanelPrefab));
        upgradePanelButtons[3].onClick.AddListener(() => ShowPanel(characterGimmickPanelPrefab));
        
        ShowPanel(monsterUpgradePanelPrefab);
        costTexts[0].text = GameManager.instance.ResourceManager.Diamond.ToString();
        costTexts[1].text = GameManager.instance.ResourceManager.Gold.ToString();
        costTexts[2].text = GameManager.instance.ResourceManager.Ticket.ToString();
    }

    private void ShowPanel(MonoBehaviour selectedPanel)
    {
        monsterUpgradePanelPrefab.gameObject.SetActive(false);
        monsterGimmickPanelPrefab.gameObject.SetActive(false);
        characterUpgradePanelPrefab.gameObject.SetActive(false);
        characterGimmickPanelPrefab.gameObject.SetActive(false);
        selectedPanel.gameObject.SetActive(true);
    }
}