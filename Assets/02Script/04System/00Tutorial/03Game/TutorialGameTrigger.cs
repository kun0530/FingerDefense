using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameTrigger : MonoBehaviour
{
    private TutorialObserver observer;

    public void SetObserver(TutorialObserver observer)
    {
        this.observer = observer;
    }

    public void OnDragStarted()
    {
        observer?.OnMonsterDragStarted(this);
    }

    public void OnDropped()
    {
        observer?.OnMonsterDropped(this);
    }

    public void OnFallSurvived()
    {
        observer?.OnMonsterSurvived(this);
    }

    public void OnDisabled()
    {
        observer?.OnTargetDisabled(this);
    }
    
    // 이 메서드는 몬스터가 드래그 후 실패한 후 살아남았을 때 호출됩니다.
    public void OnFailButSurvived()
    {
        observer?.OnMonsterSurvived(this);
    }
    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 리스트에서 제거합니다.
        Logger.Log($"{gameObject.name} is being disabled.");
        OnDisabled();
    }
}
