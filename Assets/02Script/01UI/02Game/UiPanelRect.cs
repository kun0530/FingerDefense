using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelRect : MonoBehaviour
{
    private CameraController cameraController;
    private RectTransform uiPanel;
    private Vector2 worldPoint;
    public float yRange = 2f;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        uiPanel = GetComponent<RectTransform>();
    }

    void Update()
    {
        worldPoint = new Vector2(cameraController.currentWidth / 2f, cameraController.bottomY + yRange);
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(worldPoint);
        Rect viewportRect = new Rect(0f, 0f, viewportPoint.x, viewportPoint.y);
        ApplyRectArea(viewportRect);
    }

    private void ApplyRectArea(Rect rect)
    {
        uiPanel.anchorMin = rect.min;
        uiPanel.anchorMax = rect.max;
    }
}
