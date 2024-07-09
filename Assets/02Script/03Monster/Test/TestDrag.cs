using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드래그 테스트용 스크립트이며, 터치 관련 시스템이 구축되면 삭제해야 합니다.
/// </summary>
public class TestDrag : MonoBehaviour
{
    private bool isDragging = false;
    private GameObject dragTarget;
    private float targetOriginY;

    private void Update()
    {
        if (!isDragging && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit;
            // LayerMask layerMask = 1 << LayerMask.NameToLayer("Monster");
            var mouseScreenPos = Input.mousePosition;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            hit = Physics2D.Raycast(mouseWorldPos, transform.forward, Mathf.Infinity);
            if (hit)
            {
                var target = hit.collider.gameObject;
                if (target.TryGetComponent<MonsterController>(out var controller)
                && controller.TryTransitionState<DragState>())
                {
                    dragTarget = target;
                    targetOriginY = dragTarget.transform.position.y;
                    isDragging = true;
                }
            }
        }

        if (isDragging)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = dragTarget.transform.position.z;
            dragTarget.transform.position = pos;

            if (Input.GetMouseButtonUp(0))
            {
                if (dragTarget.TryGetComponent<MonsterController>(out var controller)
                && controller.TryTransitionState<FallState>())
                {
                    dragTarget = null;
                    isDragging = false;
                    targetOriginY = 0f;
                }
            }
        }
    }
}