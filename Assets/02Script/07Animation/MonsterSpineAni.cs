using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

[RequireComponent(typeof(MonsterController))]
public class MonsterSpineAni : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] monsterAnimClip;
    
    private Spine.AnimationState spineAnimationState;
    
    private MonsterController monsterController;
    
    private MonsterState monsterState;
    private string currentAnimation;
    
    public enum MonsterState
    {
        ATTACK,
        BESHOT,
        DEAD,
        IDLE,
        LAYDOWN_AFTER,
        WALK,
    }

    private void Awake()
    {
        skeletonAnimation = GetComponentInChildren(typeof(SkeletonAnimation)) as SkeletonAnimation;
    }

    private void Start()
    {
        monsterController = TryGetComponent(out MonsterController controller) ? controller : null;
        
        spineAnimationState = skeletonAnimation.AnimationState;
        SetAnimation(MonsterState.IDLE, true, 0.3f);
        
    }
    
    public void SetAnimation(MonsterState state, bool loop, float timeScale)
    {
        if (monsterAnimClip.Length <= (int)state)
        {
            Logger.LogError("해당 애니메이션 클립이 없습니다.");
            return;
        }
        
        if (currentAnimation == state.ToString())
            return;
        
        currentAnimation = state.ToString();
        spineAnimationState.SetAnimation(0, monsterAnimClip[(int)state], loop).TimeScale = timeScale;
    }

    private void Update()
    {
        // if (monsterController == null)
        //     return;
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAnimation(MonsterState.IDLE, true, 0.3f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAnimation(MonsterState.WALK, true, 0.3f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAnimation(MonsterState.ATTACK, true, 0.3f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetAnimation(MonsterState.BESHOT, true, 0.3f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetAnimation(MonsterState.LAYDOWN_AFTER, true, 0.3f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetAnimation(MonsterState.DEAD, true, 0.3f);
        }
    }
}
