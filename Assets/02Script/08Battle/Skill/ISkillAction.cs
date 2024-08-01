using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillAction
{
    bool ApplySkillAction(GameObject target);
    bool EnterSkillArea(GameObject target, SkillArea area);
    bool ExitSkillArea(GameObject target, SkillArea area);
}
