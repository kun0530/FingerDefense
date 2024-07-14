using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;


public enum CharacterAnimState
{
    Idle,
    Walk,
    Attack,
    Hit,
    Die
}

public class CharacterSpineAni : MonoBehaviour
{
    [SerializeField]
    private SkeletonAnimation[] skeletonAnimation;
    public AnimationReferenceAsset[] characterAnimClip;
    private Spine.AnimationState spineAnimationState;
    
    private CharacterController characterController;
    private void Awake()
    {
        skeletonAnimation = GetComponentsInChildren<SkeletonAnimation>();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        spineAnimationState.Event += HandleSpineEvent;
    }
    private void HandleSpineEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if(e.Data.Name == "Hit")
        {
            ChangeAnimationLayer();
        }
    }
    
    private void ChangeAnimationLayer()
    {
        foreach (var t in skeletonAnimation)
        {
            t.AnimationState.SetAnimation(1, characterAnimClip[3], false);
        }
    }
    private void Update()
    {
        if(characterController)
        {
            
        }
    }
    
    public void SetAnimation(CharacterAnimState state)
    {
        switch (state)
        {
            case CharacterAnimState.Idle:
                foreach (var t in skeletonAnimation)
                {
                    t.AnimationState.SetAnimation(0, characterAnimClip[0], true);
                }
                break;
            case CharacterAnimState.Walk:
                foreach (var t in skeletonAnimation)
                {
                    t.AnimationState.SetAnimation(0, characterAnimClip[1], true);
                }
                break;
            case CharacterAnimState.Attack:
                foreach (var t in skeletonAnimation)
                {
                    t.AnimationState.SetAnimation(0, characterAnimClip[2], true);
                }
                break;
            case CharacterAnimState.Hit:
                foreach (var t in skeletonAnimation)
                {
                    t.AnimationState.SetAnimation(0, characterAnimClip[3], true);
                }
                break;
            case CharacterAnimState.Die:
                foreach (var t in skeletonAnimation)
                {
                    t.AnimationState.SetAnimation(0, characterAnimClip[4], true);
                }
                break;
        }
    }
    
    
    
    
}
