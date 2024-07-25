using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour
{
    public BaseSkill normalAttack;
    public BaseSkill skillAttack;

    private BaseSkill currentAttack;

    private CharacterSpineAni characterAni;
    private bool isAnimationEnded = true;

    private TrackEntry attactTrackEntry;

    private void Awake()
    {
        characterAni = GetComponent<CharacterSpineAni>();
        characterAni.SetAnimation(CharacterSpineAni.CharacterState.IDLE, true, 1f);
    }

    private void FixedUpdate()
    {
        UpdateSkill(normalAttack);
        UpdateSkill(skillAttack);
    }

    private void UpdateSkill(BaseSkill skill)
    {
        if (skill == null)
            return;

        skill.TimerUpdate();

        if (!isAnimationEnded)
            return;

        if (skill.IsSkillReady)
        {
            if (skill.UseSkill())
            {
                attactTrackEntry = characterAni.SetAnimation(CharacterSpineAni.CharacterState.ATTACK, false, 1f);
                if (attactTrackEntry != null)
                    attactTrackEntry.Complete += AttackEnd;
                currentAttack = skill;
                isAnimationEnded = false;
            }
        }
    }

    private void AttackEnd(TrackEntry entry)
    {
        if (attactTrackEntry != null)
            attactTrackEntry.Complete -= AttackEnd;
        characterAni.SetAnimation(CharacterSpineAni.CharacterState.IDLE, true, 1f);

        isAnimationEnded = true;
    }
}