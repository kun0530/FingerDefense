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

    public DragState(MonsterController controller)
    {
        this.controller = controller;

        renderer = controller.GetComponentInChildren<MeshRenderer>();
        collider = controller.GetComponent<Collider2D>();
    }

    public void Enter()
    {
        controller.targetFallY = controller.transform.position.y;
        isReachHeight = false;

        var stageManager = controller.stageManager;
        if (stageManager)
        {
            var limitX1 = stageManager.monsterSpawner.moveTarget1.position.x;
            var limitX2 = controller.stageManager.monsterSpawner.moveTarget2.position.x;
            limitX = limitX1 > limitX2 ? limitX1 : limitX2;
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

        controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.LAYDOWN_AFTER, true, 1f);
        TimeScaleController.ChangeTimeSclae(0.1f, 0.25f);
        if (Camera.main != null && Camera.main.TryGetComponent<CameraController>(out var cameraController))
        {
            cameraController.ZoomOutCamera();
        }
    }

    public void Update()
    {
        if (dragAndDrop.IsDragging)
        {
            var pos = Camera.main!.ScreenToWorldPoint(dragAndDrop.GetPointerPosition());
            pos.z = 0f;
            if (pos.y <= controller.targetFallY)
                pos.y = controller.targetFallY;
            if (pos.x <= limitX)
                pos.x = limitX;
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
    }
}
