using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMask : TutorialBase
{
    public GameObject Mask;
    public Button NextButton;
    public override void Enter()
    {
        Mask.gameObject.SetActive(true);
        Mask.gameObject.transform.SetAsLastSibling();
    }

    public override void Execute(TutorialController controller)
    {
        //버튼이 눌렀을때 다음 튜토리얼로 넘어가게 설정
        NextButton.onClick.AddListener(controller.SetNextTutorial);
    }

    public override void Exit()
    {
        Mask.gameObject.SetActive(false);
        Mask.gameObject.transform.SetAsFirstSibling();
    }
    
    
}                    
