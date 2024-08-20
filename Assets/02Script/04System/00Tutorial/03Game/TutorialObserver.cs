using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialObserver : TutorialBase
{
    public List<TutorialGameTrigger> observedMonsters = new List<TutorialGameTrigger>();

    public TextMeshProUGUI tutorialText;
    public Image tutorialImage;
    private StringTable stringTable;

    public GameObject monsterSpawnParent;

    public bool isRemove = false;
    public bool isAdd = false;

    public bool isDragLock = false;
    public bool isState = false;
    
    public void Awake()
    {
        stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);
    }

    private void Start()
    {
        // 처음에 이미 존재하는 자식들을 모두 추가
        CheckAndAddMonsters();
        // 주기적으로 확인하는 UniTask 실행
        MonitorMonstersAsync().Forget(); // Forget을 사용하여 비동기 작업을 백그라운드에서 실행
    }

    private async UniTaskVoid MonitorMonstersAsync()
    {
        while (true) // 계속 반복
        {
            CheckAndAddMonsters();
            ControlMonsterStates();
            await UniTask.Delay(100); 
        }
    }

    private void CheckAndAddMonsters()
    {
        var monsterTriggers = monsterSpawnParent.GetComponentsInChildren<TutorialGameTrigger>();
        foreach (var monsterTrigger in monsterTriggers)
        {
            AddMonster(monsterTrigger);
        }
    }

    public void AddMonster(TutorialGameTrigger monsterTrigger)
    {
        if (!observedMonsters.Contains(monsterTrigger))
        {
            observedMonsters.Add(monsterTrigger);
            monsterTrigger.SetObserver(this);
        }

        if (observedMonsters.Count == 1)
        {
            if (isAdd)
            {
                var controller = GetComponentInParent<TutorialController>();
                controller.SetNextTutorial();    
            }
        }
    }

    public override void Enter()
    {
            
    }

    public override void Execute(TutorialController controller)
    {
        if(isDragLock && isState)
        {
            controller.SetNextTutorial();
        }
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
        }
        
        var controller = GetComponentInParent<TutorialController>();
        controller.SetNextTutorial();
    }

    public void OnMonsterDragStarted(TutorialGameTrigger tutorialGameTrigger)
    {
    }

    public void OnMonsterDropped(TutorialGameTrigger tutorialGameTrigger)
    {
    
    }

    public void OnMonsterSurvived(TutorialGameTrigger tutorialGameTrigger)
    {
    
    }

    private void DisplayTutorialText(string message)
    {
        tutorialText.text = message;
        tutorialText.gameObject.SetActive(true);
        tutorialText.DOFade(1, 0.5f).From(0); // 텍스트가 서서히 나타나도록 애니메이션 추가
        tutorialImage.gameObject.SetActive(true);
        tutorialImage.DOFade(1, 0.5f).From(0); // 이미지가 서서히 나타나도록 애니메이션 추가
        
    }

    private void ControlMonsterStates()
    {
        foreach (var monsterTrigger in observedMonsters)
        {
            var monsterController = monsterTrigger.gameObject.GetComponent<MonsterController>();
            if (monsterController != null)
            {
                // isDragLock가 true이면 드래그 유형을 보스로 설정
                if (isDragLock)
                {
                    monsterController.Status.Data.DragType = (int)MonsterData.DragTypes.BOSS;
                }
                // isState가 true이면 상태를 특정 상태로 전환 (예: PatrolState)
                if (isState)
                {
                    monsterController.IsTutorialMonster = false;
                }
                
            }
            else
            {
                return;
            }
        }
    }
}
