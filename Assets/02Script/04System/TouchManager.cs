using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    public bool isMultiTouch;
    private Camera mainCamera;
    private GameObject[] selectedObject;

    private void Awake()
    {
        mainCamera = Camera.main;
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
        if (Touchscreen.current.touches[0].press.isPressed)
        {
            
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);
            if (!hitCollider.CompareTag("Draggable")) return;
            if (hitCollider != null)
            {
                Debug.Log("Hit object: " + hitCollider.name);
            }
            if(hitCollider.CompareTag("Draggable") && hitCollider != null)
            {
                Debug.Log("Draggable object: " + hitCollider.name);
                var transform1 = hitCollider.transform;
                transform1.position = new Vector3(worldPosition.x, worldPosition.y, transform1.position.z);
                
            }
        }
    }

    private void HandleMultiTouch()
    {
        if (Touchscreen.current.touches[0].press.isPressed || Touchscreen.current.touches[1].press.isPressed)
        {
            Vector2 touchPosition1 = Touchscreen.current.touches[0].position.ReadValue();
            Vector2 touchPosition2 = Touchscreen.current.touches[1].position.ReadValue();

            Vector3 worldPosition1 = mainCamera.ScreenToWorldPoint(touchPosition1);
            Vector3 worldPosition2 = mainCamera.ScreenToWorldPoint(touchPosition2);

            Collider2D hitCollider1 = Physics2D.OverlapPoint(worldPosition1);
            Collider2D hitCollider2 = Physics2D.OverlapPoint(worldPosition2);
            if(!hitCollider1.CompareTag("Draggable") || !hitCollider2.CompareTag("Draggable")) return;
            if (hitCollider1 != null && hitCollider1.CompareTag("Draggable"))
            {
                Debug.Log("Hit object 1: " + hitCollider1.name);
                var transform1 = hitCollider1.transform;
                transform1.position = new Vector3(worldPosition1.x, worldPosition1.y, transform1.position.z);
                
            }

            if (hitCollider2 != null && hitCollider2.CompareTag("Draggable"))
            {
                Debug.Log("Hit object 2: " + hitCollider2.name);
                var transform1 = hitCollider2.transform;
                transform1.position = new Vector3(worldPosition2.x, worldPosition2.y, transform1.position.z);
            }
        }
       
    }
}
