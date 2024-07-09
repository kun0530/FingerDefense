using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// 테스트용 코드 삭제예정
/// </summary>

public class Draggable : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 startDragPosition; // 드래그 시작 위치
    private TextMeshProUGUI dragDistanceText;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public bool IsDragging => isDragging;

    public void SetDragDistanceText(TextMeshProUGUI text)
    {
        dragDistanceText = text;
    }

    public void StartDragging(Vector2 touchPosition)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane));
        offset = transform.position - worldPosition;
        startDragPosition = transform.position; // 드래그 시작 위치 저장
        isDragging = true;
        DragObject().Forget();
    }

    public void StopDragging(Vector2 touchPosition)
    {
        isDragging = false;
        Vector3 endDragPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane));
        float dragDistance = endDragPosition.y - startDragPosition.y; // 드래그된 거리 계산
        if (dragDistanceText != null)
        {
            dragDistanceText.text = "Dragged Distance Upwards: " + dragDistance.ToString("F2"); // TextMeshPro에 표시
        }
    }

    public void Drag(Vector2 touchPosition)
    {
        if (isDragging)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane)) + offset;
            transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
        }
    }

    private async UniTaskVoid DragObject()
    {
        while (isDragging)
        {
            await UniTask.Yield();
        }
    }
}