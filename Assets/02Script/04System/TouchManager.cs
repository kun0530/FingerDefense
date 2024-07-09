using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// 테스트용 코드 삭제예정
/// </summary>
public class TouchManager : MonoBehaviour
{
    public bool isMultiTouch;
    private Camera mainCamera;

    private Draggable[] draggableObjects;
    private Draggable[] activeDraggables = new Draggable[2];
    private TextMeshProUGUI dragDistanceText;

    private void Awake()
    {
        mainCamera = Camera.main;
        draggableObjects = FindObjectsOfType<Draggable>();
        dragDistanceText = GameObject.Find("DragDistanceText").GetComponent<TextMeshProUGUI>();
    }

    public void SetSingleTouch()
    {
        isMultiTouch = false;
    }

    public void SetMultiTouch()
    {
        isMultiTouch = true;
    }

    private void Update()
    {
        if (Touchscreen.current == null)
        {
            Debug.LogWarning("Touchscreen not found. Make sure a touchscreen device is connected.");
            return;
        }

        if (isMultiTouch)
        {
            HandleMultiTouch();
        }
        else
        {
            HandleSingleTouch();
        }
    }

    private void HandleSingleTouch()
    {
        if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].press.isPressed)
        {
            if (activeDraggables[0] == null)
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane)), Vector2.zero);
                if (hit.collider != null)
                {
                    Draggable draggable = hit.collider.GetComponent<Draggable>();
                    if (draggable != null && !draggable.IsDragging)
                    {
                        ClearPreviousDistance();
                        draggable.SetDragDistanceText(dragDistanceText); // 드래그 시작 시 TextMeshPro 객체 설정
                        draggable.StartDragging(touchPosition);
                        activeDraggables[0] = draggable;
                    }
                }
            }
        }

        if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].press.wasReleasedThisFrame)
        {
            if (activeDraggables[0] != null)
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                activeDraggables[0].StopDragging(touchPosition);
                activeDraggables[0] = null;
            }
        }

        if (activeDraggables[0] != null)
        {
            activeDraggables[0].Drag(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    private void HandleMultiTouch()
    {
        int touchCount = Touchscreen.current.touches.Count;
        for (int i = 0; i < touchCount && i < 2; i++)
        {
            if (Touchscreen.current.touches[i].press.isPressed)
            {
                if (activeDraggables[i] == null)
                {
                    Vector2 touchPosition = Touchscreen.current.touches[i].position.ReadValue();
                    RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane)), Vector2.zero);
                    if (hit.collider != null)
                    {
                        Draggable draggable = hit.collider.GetComponent<Draggable>();
                        if (draggable != null && !draggable.IsDragging)
                        {
                            ClearPreviousDistance();
                            draggable.SetDragDistanceText(dragDistanceText); // 드래그 시작 시 TextMeshPro 객체 설정
                            draggable.StartDragging(touchPosition);
                            activeDraggables[i] = draggable;
                        }
                    }
                }
            }

            if (Touchscreen.current.touches[i].press.wasReleasedThisFrame)
            {
                if (activeDraggables[i] != null)
                {
                    Vector2 touchPosition = Touchscreen.current.touches[i].position.ReadValue();
                    activeDraggables[i].StopDragging(touchPosition);
                    activeDraggables[i] = null;
                }
            }

            if (activeDraggables[i] != null)
            {
                activeDraggables[i].Drag(Touchscreen.current.touches[i].position.ReadValue());
            }
        }
    }

    private void ClearPreviousDistance()
    {
        if (dragDistanceText != null)
        {
            dragDistanceText.text = "";
        }
    }
}
