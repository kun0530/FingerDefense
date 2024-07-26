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
    private PlayerCharacterController controller;

    private void Awake()
    {
        characterAni = GetComponent<CharacterSpineAni>();
        controller = GetComponent<PlayerCharacterController>();
    }

    private void OnEnable()
    {
        characterAni.SetAnimation(CharacterSpineAni.CharacterState.IDLE, true, 1f);
        isAnimationEnded = true;
    }

    private void FixedUpdate()
    {
        if (controller && controller.IsDead)
            return;
        
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
                currentAttack = skill;
                SkillStart();
            }
        }
    }

    private void SkillStart()
    {
        attactTrackEntry = characterAni.SetAnimation(CharacterSpineAni.CharacterState.ATTACK, false, 1f);
        if (attactTrackEntry != null)
            attactTrackEntry.Complete += SkillEnd;
        isAnimationEnded = false;
    }

    private void SkillEnd(TrackEntry entry)
    {
        if (attactTrackEntry != null)
            attactTrackEntry.Complete -= SkillEnd;
        characterAni.SetAnimation(CharacterSpineAni.CharacterState.IDLE, true, 1f);

        isAnimationEnded = true;
    }
}