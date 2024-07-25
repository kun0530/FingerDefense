using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private float targetWidth = 20f;
    [SerializeField] private float targetHeight = 10f;
    [SerializeField] private float bottomY = -5f;

    [SerializeField] private bool isExistLetterBox = true;
    [SerializeField] private GameObject letterBoxGameObject;
    private RectTransform letterBoxCanvasRect;
    private Image[] letterBoxes;

    private void Start()
    {
        mainCamera = Camera.main;
        letterBoxCanvasRect = letterBoxGameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        letterBoxes = letterBoxGameObject.GetComponentsInChildren<Image>();

        if (isExistLetterBox)
            AdjustCameraUsingLetterBox();
        else
            AdjustCamera();
    }

    private void Update()
    {
        if (isExistLetterBox)
            AdjustCameraUsingLetterBox();
        else
            AdjustCamera();
    }

    private void AdjustCamera()
    {
        var normalRect = new Rect(0f, 0f, 1f, 1f);
        mainCamera.rect = normalRect;
        SetLetterBoxInactive();

        var aspectRatio = (float)Screen.width / (float)Screen.height;
        var orthographicSize = targetWidth / (aspectRatio * 2f);
        mainCamera.orthographicSize = orthographicSize;

        var cameraPositionY = bottomY + orthographicSize;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraPositionY, mainCamera.transform.position.z);
    }

    private void AdjustCameraUsingLetterBox()
    {
        float screenAspectRatio = (float)Screen.width / (float)Screen.height;
        float targetAspectRatio = targetWidth / targetHeight;

        if (screenAspectRatio >= targetAspectRatio)
        {
            // 좌우 레터박스
            float viewportWidth = targetAspectRatio / screenAspectRatio;
            var adjustedRect = new Rect((1f - viewportWidth) / 2f, 0f, viewportWidth, 1f);
            mainCamera.rect = adjustedRect;
            SetLetterBoxSize((1f - viewportWidth) / 2f, 1f);
        }
        else
        {
            // 상하 레터박스
            float viewportHeight = screenAspectRatio / targetAspectRatio;
            var adjustedRect = new Rect(0f, (1f - viewportHeight) / 2f, 1f, viewportHeight);
            mainCamera.rect = adjustedRect;
            SetLetterBoxSize(1f, (1f - viewportHeight) / 2f);
        }

        var orthographicSize = targetHeight / 2f;
        mainCamera.orthographicSize = orthographicSize;
        var cameraPositionY = bottomY + orthographicSize;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraPositionY, mainCamera.transform.position.z);
    }

    private void SetLetterBoxSize(float widthPercentage, float heightPercentage)
    {
        float newWidth = letterBoxCanvasRect.rect.width * widthPercentage;
        float newHeight = letterBoxCanvasRect.rect.height * heightPercentage;

        foreach (var letterBox in letterBoxes)
        {
            letterBox.gameObject.SetActive(true);
            letterBox.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }
    }

    private void SetLetterBoxInactive()
    {
        foreach (var letterBox in letterBoxes)
        {
            letterBox.gameObject.SetActive(false);
        }
    }

    private void SetRect(RectTransform rectTransform, Rect rect)
    {
        rectTransform.anchorMin = new Vector2(rect.x, rect.y);
        rectTransform.anchorMax = new Vector2(rect.x + rect.width, rect.y + rect.height);
    }

    public static void SetLetterBoxRect(RectTransform rectTransform)
    {
        var rect = Camera.main.rect;

        rectTransform.anchorMin = new Vector2(rect.x, rect.y);
        rectTransform.anchorMax = new Vector2(rect.x + rect.width, rect.y + rect.height);
    }
}