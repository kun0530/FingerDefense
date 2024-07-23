using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour
{
    public BaseSkill baseSkill;

    private void Update()
    {
        if (baseSkill == null)
            return;

        baseSkill.TimerUpdate();

        if (baseSkill.IsSkillReady)
        {
            baseSkill.UseSkill();
        }
    }
}