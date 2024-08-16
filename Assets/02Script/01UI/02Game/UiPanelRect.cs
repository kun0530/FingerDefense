using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelRect : MonoBehaviour
{
    private CameraController cameraController;
    private RectTransform uiPanel;
    private Vector2 worldPoint;
    public float yRange = 2f;

    private RectTransform canvasRect;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        var canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        uiPanel = GetComponent<RectTransform>();
    }

    void Update()
    {
        worldPoint = new Vector2(cameraController.currentWidth / 2f, cameraController.bottomY + yRange);
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(worldPoint);
        uiPanel.sizeDelta = new Vector2(uiPanel.sizeDelta.x, canvasRect.rect.height * viewportPoint.y);
    }
}