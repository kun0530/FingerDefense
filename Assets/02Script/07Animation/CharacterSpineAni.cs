using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

[RequireComponent(typeof(PlayerCharacterController))]
public class CharacterSpineAni : MonoBehaviour
{
    public SkeletonAnimation[] skeletonAnimation;
    public AnimationReferenceAsset[] characterAnimClip;

    private Spine.AnimationState spineAnimationState;
    private PlayerCharacterController characterController;

    private CharacterState characterState;
    private string currentAnimation;

    [Tooltip("케이 캐릭터의 방패에 대한 레이어를 담아두는 변수")]
    private int originalSortingOrder;

    public enum CharacterState
    {
        ATTACK,
        ATTACK_SHEILD,
        HIT,
        IDLE,
        PASSOUT,
        RUN
    }

    private void Awake()
    {
        skeletonAnimation = GetComponentsInChildren<SkeletonAnimation>();
        if (skeletonAnimation.Length > 2)
        {
            originalSortingOrder = skeletonAnimation[2].GetComponent<MeshRenderer>().sortingOrder;
        }
        else
        {
            Logger.LogWarning("해당 캐릭터는 방패가 없습니다.");
        }
    }

    private void Start()
    {
        characterController = TryGetComponent(out PlayerCharacterController controller) ? controller : null;
        if (skeletonAnimation.Length > 2)
        {
            skeletonAnimation[2].AnimationState.Event += HandleSpineEvent;
        }
        else
        {
            Logger.LogWarning("해당 캐릭터는 방패가 없습니다.");
        }
        
        SetAnimation(CharacterState.IDLE, true, 0.3f);
    }

    private void HandleSpineEvent(TrackEntry trackEntry, Spine.Event e)
    {
        switch (currentAnimation)
        {
            case "HIT":
            case "ATTACK":
            {
                if (e.Data.Name == "End")
                {
                    if (skeletonAnimation.Length > 2)
                    {
                        skeletonAnimation[2].TryGetComponent(out MeshRenderer renderer);
                        renderer.sortingOrder = originalSortingOrder;
                    }
                    else
                    {
                        return;
                    }
                }
            }
                break;
        }
    }


    private void Update()
    {
        // if (!characterController)
        //     return;

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SetAnimation(CharacterState.ATTACK, true, 0.3f);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SetAnimation(CharacterState.ATTACK_SHEILD, true, 0.3f);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SetAnimation(CharacterState.HIT, true, 0.3f);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            SetAnimation(CharacterState.IDLE, true, 0.3f);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            SetAnimation(CharacterState.PASSOUT, true, 0.3f);
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            SetAnimation(CharacterState.RUN, true, 0.3f);
        }
    }

    public void SetAnimation(CharacterState state, bool loop, float timeScale)
    {
        var clip = characterAnimClip[(int)state];
        if (clip.name.Equals(currentAnimation))
            return;

        foreach (var skeleton in skeletonAnimation)
        {
            skeleton.AnimationState.SetAnimation(0, clip, loop);
            skeleton.timeScale = timeScale;
        }

        currentAnimation = clip.name;
        SetCharacterState(state);
    }

    private void SetCharacterState(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.HIT:
            {
                if (skeletonAnimation.Length > 2)
                {
                    skeletonAnimation[2].TryGetComponent(out MeshRenderer renderer);
                    renderer.sortingOrder = 1;
                }
            }
                break;
            case CharacterState.ATTACK:
            {
                if (skeletonAnimation.Length > 2)
                {
                    skeletonAnimation[2].TryGetComponent(out MeshRenderer renderer);
                    renderer.sortingOrder = -1;
                }

            }
                break;
            case CharacterState.PASSOUT:
            {
                if (skeletonAnimation.Length > 2)
                {
                    skeletonAnimation[2].TryGetComponent(out MeshRenderer renderer);
                    renderer.sortingOrder = -1;
                }
            }
                break;
            }
        }
    }

    

