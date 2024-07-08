using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = false;
    private Transform targetObject;
    public string draggableTag = "Draggable"; // 드래그 가능한 오브젝트의 태그 설정
    
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

    

    // 태그를 통해 드래그 가능한 오브젝트를 찾아서 드래그 가능 여부를 판단
    // 인터페이스가 있는지 여부를 가져와야함 
    private void OnPointerDown(InputAction.CallbackContext context)
    {
        Vector2 pointerPosition = GetPointerPosition(context);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(pointerPosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);

        if (hitCollider != null && hitCollider.CompareTag(draggableTag))
        {
            //if(this==null) return;
            targetObject = hitCollider.transform;
            var position = targetObject.position;
            offset = position - new Vector3(worldPosition.x, worldPosition.y, position.z);
            
            isDragging = true;
        }
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        isDragging = false;
        targetObject = null;
        
    }

    private void OnPointerDrag(InputAction.CallbackContext context)
    {
        if (isDragging && targetObject != null)
        {
            Vector2 pointerPosition = GetPointerPosition(context);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(pointerPosition);
            targetObject.position = new Vector3(worldPosition.x, worldPosition.y, targetObject.position.z) + offset;
        }
    }

    private Vector2 GetPointerPosition(InputAction.CallbackContext context)
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


}
