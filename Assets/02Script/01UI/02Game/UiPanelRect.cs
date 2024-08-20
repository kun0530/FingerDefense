using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelRect : MonoBehaviour
{
    private GameCameraController cameraController;
    private RectTransform uiPanel;
    private Vector2 worldPoint;
    public float yRange = 2f;

    private RectTransform canvasRect;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<GameCameraController>();
        var canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        uiPanel = GetComponent<RectTransform>();

        AdjustPanelSize();
    }

    private void AdjustPanelSize()
    {
        // worldPoint = new Vector2(cameraController.currentWidth / 2f, cameraController.bottomY + yRange);
        // Vector2 viewportPoint = Camera.main.WorldToViewportPoint(worldPoint);
        // uiPanel.sizeDelta = new Vector2(uiPanel.sizeDelta.x, canvasRect.rect.height * viewportPoint.y);

        float aspectRatio = Camera.main.aspect;
        float currentHeight = cameraController.currentWidth / aspectRatio;
        uiPanel.sizeDelta = new Vector2(uiPanel.sizeDelta.x, canvasRect.rect.height * yRange / currentHeight);
    }
}