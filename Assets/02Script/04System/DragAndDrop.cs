using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    private Camera mainCamera;
    private float targetOriginY;
    public bool IsDragging { get; private set; } = false;

    private GameObject draggingObject;
    private const float autoDropTime = 2.0f; // 드래그 시작 후 2초 뒤에 자동으로 놓기

    

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnClick += OnPointerDown;
        InputManager.Instance.OnRelease += OnPointerUp;
        InputManager.Instance.OnDrag += OnPointerDrag;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnClick -= OnPointerDown;
        InputManager.Instance.OnRelease -= OnPointerUp;
        InputManager.Instance.OnDrag -= OnPointerDrag;
    }

    private void OnPointerDown(InputAction.CallbackContext context)
    {
        if (!IsDragging)
        {
            var mouseScreenPos = GetPointerPosition();
            var mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit)
            {
                var target = hit.collider.gameObject;
                if (target.TryGetComponent<MonsterController>(out var controller))
                {
                    if (controller.TryTransitionState<DragState>())
                    {
                        targetOriginY = target.transform.position.y;
                        IsDragging = true;
                        draggingObject = target;

                        // 일정 시간이 지나면 자동으로 놓기
                        AutoDropAfterTime(autoDropTime).Forget();
                    }
                    else
                    {
                        Logger.Log("This GameObject cannot be dragged!");
                    }
                }
            }
        }
        else
        {
            Logger.Log("Already dragging!");
        }
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        if (IsDragging)
        {
            DropObject();
        }
    }

    private void OnPointerDrag(InputAction.CallbackContext context)
    {
        if (IsDragging)
        {
            var pos = mainCamera.ScreenToWorldPoint(GetPointerPosition());
            var transform1 = draggingObject.transform;
            pos.z = transform1.position.z;
            transform1.position = pos;
        }
    }

    public Vector2 GetPointerPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else
        {
            return Mouse.current.position.ReadValue();
        }
    }
    
   
    
    private void DropObject()
    {
        IsDragging = false;
        FallObject(draggingObject, targetOriginY).Forget();
        targetOriginY = 0f;
        draggingObject = null;
    }

    private async UniTask FallObject(GameObject target, float targetHeight)
    {
        const float gravity = -9.8f;
        var velocity = 0f;

        while (target != null)
        {
            if (target.transform.position.y <= targetHeight)
            {
                var position = target.transform.position;
                position = new Vector3(position.x, targetHeight, position.z);
                target.transform.position = position;
                if (target.TryGetComponent<MonsterController>(out var controller))
                {
                    controller.TryTransitionState<FallState>();
                }
                break;
            }

            velocity += gravity * Time.deltaTime;
            target.transform.position += new Vector3(0, velocity * Time.deltaTime, 0);
            await UniTask.Yield();
        }
    }

    private async UniTask AutoDropAfterTime(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        if (IsDragging)
        {
            DropObject();
        }
    }

    
}
