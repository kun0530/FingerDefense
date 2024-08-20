using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragState : IState
{
    private MonsterController controller;
    private MeshRenderer renderer;
    private Collider2D collider;

    private DragAndDrop dragAndDrop;

    private bool isReachHeight;

    private float limitX;
    
    private TutorialGameTrigger tutorialGameTrigger;

    public DragState(MonsterController controller)
    {
        this.controller = controller;

        renderer = controller.GetComponentInChildren<MeshRenderer>();
        collider = controller.GetComponent<Collider2D>();
       
        //To-Do Tutorial 몬스터 이동 상태 추가
        if (controller.IsTutorialMonster)
        {
            tutorialGameTrigger = controller.GetComponent<TutorialGameTrigger>();
        }
    }

    public void Enter()
    {
        controller.targetFallY = controller.transform.position.y;
        isReachHeight = false;

        var stageManager = controller.stageManager;
        if (stageManager)
        {
            limitX = Utils.GetXFromLinear(stageManager.castleRightTopPos.position, stageManager.castleLeftBottomPos.position, controller.transform.position.y);
        }
        else
        {
            limitX = controller.moveTargetPos.x;
        }

        if (controller.attackTarget)
            controller.attackTarget.TryRemoveMonster(controller);

        dragAndDrop = GameObject.FindGameObjectWithTag("InputManager").GetComponent<DragAndDrop>();

        collider.enabled = false;
        renderer.sortingOrder = 1;
        controller.shadowImage?.gameObject.SetActive(false);

        controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.LAYDOWN_AFTER, true, 1f);
        TimeScaleController.ChangeTimeSclae(0.1f, 0.25f);
        if (Camera.main != null && Camera.main.TryGetComponent<CameraController>(out var cameraController))
        {
            cameraController.ZoomOutCamera();
        }
        
        // 드래그 시작 시점에서 튜토리얼 몬스터인 경우에만 OnDragStarted 호출
        if (controller.IsTutorialMonster && tutorialGameTrigger != null)
        {
            tutorialGameTrigger.OnDragStarted();
        }
    }

    public void Update()
    {
        if (dragAndDrop.IsDragging)
        {
            var pos = Camera.main!.ScreenToWorldPoint(dragAndDrop.GetPointerPosition());
            if (pos.y <= controller.targetFallY)
                pos.y = controller.targetFallY;
            if (pos.x <= limitX)
                pos.x = limitX;
            pos.z = pos.y;
            controller.transform.position = pos;

            if (!isReachHeight && pos.y > controller.Status.Data.Height + controller.targetFallY)
            {
                Handheld.Vibrate();
                isReachHeight = true;
            }
            else if (isReachHeight && pos.y < controller.Status.Data.Height + controller.targetFallY)
            {
                isReachHeight = false;
            }
        }
        else
        {
            controller.TryTransitionState<FallState>();
        }
    }

    public void Exit()
    {
        renderer.sortingOrder = 0;
        
        if (Time.timeScale == 0f)
            return;

        TimeScaleController.SetTimeScale(1f);
        if (Camera.main != null && Camera.main.TryGetComponent<CameraController>(out var cameraController))
        {
            cameraController.ResetCamera();
        }

        if (controller.transform.position.x <= limitX)
        {
            var pos = controller.transform.position;
            pos.x = limitX;
            controller.transform.position = pos;
        }
    }
}
