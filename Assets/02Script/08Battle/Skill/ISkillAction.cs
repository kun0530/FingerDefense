using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillAction
{
    void ApplySkillAction(IDamageable damageable);
}
