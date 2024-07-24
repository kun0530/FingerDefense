using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour
{
    public BaseSkill normalAttack;
    public BaseSkill skillAttack;

    private Queue<BaseSkill> readySkills = new();
    private BaseSkill currentAttack;

    private CharacterSpineAni characterAni;
    private bool isAnimationEnded = true;

    private TrackEntry attactTrackEntry;

    private void Awake()
    {
        characterAni = GetComponent<CharacterSpineAni>();
    }

    private void Update()
    {
        UpdateSkill(normalAttack);
        UpdateSkill(skillAttack);

        UseReadySkill();
    }

    private void UpdateSkill(BaseSkill skill)
    {
        if (skill == null || skill.IsSkillReady)
            return;

        skill.TimerUpdate();

        if (skill.IsSkillReady)
        {
            readySkills.Enqueue(skill);
        }
    }

    private void UseReadySkill()
    {
        if (!isAnimationEnded)
            return;

        if (currentAttack != null && !currentAttack.IsSkillCompleted)
            return;

        if (readySkills.Count != 0)
        {
            currentAttack = readySkills.Dequeue();
            currentAttack.UseSkill();
            attactTrackEntry = characterAni.SetAnimation(CharacterSpineAni.CharacterState.ATTACK, false, 1f);
            if (attactTrackEntry != null)
                attactTrackEntry.Complete += AttackEnd;
            isAnimationEnded = false;
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