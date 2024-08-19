using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;


public class TutorialObserver : TutorialBase
{
    [SerializeField]
    private List<TutorialGameTrigger> observedMonsters = new List<TutorialGameTrigger>();

    public TextMeshProUGUI tutorialText;
    private StringTable stringTable;

    public GameObject monsterSpawnParent;
    
    public void Awake()
    {
        stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);
    }
    
    public void AddMonster(TutorialGameTrigger monsterTrigger)
    {
        if (!observedMonsters.Contains(monsterTrigger))
        {
            observedMonsters.Add(monsterTrigger);
            monsterTrigger.SetObserver(this);
            Logger.Log("Added new monster to observedMonsters");
        }
        else
        {
            Logger.Log("Attempted to add a duplicate monster to observedMonsters");    
        }
    }

    public override void Enter()
    {
        tutorialText.text = stringTable.Get(95002.ToString());
        
    }
    
    public override void Execute(TutorialController controller)
    {
        // 실행 작업 
        Logger.Log($"Current observedMonsters count: {observedMonsters.Count}");
    }

    public override void Exit()
    {
        // 종료 작업
    }

    public void OnTargetDisabled(TutorialGameTrigger monsterTrigger)
    {
        if (observedMonsters.Contains(monsterTrigger))
        {
            observedMonsters.Remove(monsterTrigger);
            Logger.Log($"Monster removed. Remaining monsters: {observedMonsters.Count}");     
        }
        else
        {
            Logger.Log("Tried to remove a monster that was not in the list.");    
        }
        
        if (observedMonsters.Count == 0)
        {
            Logger.Log("몬스터가 모두 제거되었습니다.");
            var controller = GetComponentInParent<TutorialController>();
            controller.SetNextTutorial();
        }
        else
        {
            Logger.Log("There are still monsters remaining.");
        }
    }

    public void OnMonsterDragStarted(TutorialGameTrigger tutorialGameTrigger)
    {
        DisplayTutorialText(stringTable.Get("95022"));    
    }

    public void OnMonsterDropped(TutorialGameTrigger tutorialGameTrigger)
    {
        DisplayTutorialText(stringTable.Get("95012"));    
    }

    public void OnMonsterSurvived(TutorialGameTrigger tutorialGameTrigger)
    {
        DisplayTutorialText(stringTable.Get("95032"));    
    }
    
    private void DisplayTutorialText(string message)
    {
        tutorialText.text = message;
        tutorialText.gameObject.SetActive(true);
        tutorialText.DOFade(1, 0.5f).From(0); // 텍스트가 서서히 나타나도록 애니메이션 추가
    }
}
