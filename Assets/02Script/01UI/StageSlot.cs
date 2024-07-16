using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSlot : MonoBehaviour
{
    public int stageNum;
    public Image MonsterImage;
    
    public void SetSlot(int stageNum)
    {
        this.stageNum = stageNum;
    }
    
    public void OnClickSlot()
    {
        //덱 편성 UI 호출
        
    }
}
