using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float targetWidth = 20f;
    [SerializeField] private float targetHeight = 10f;
    [SerializeField] private float bottomY = -5f;

    [SerializeField] private bool isExistLetterBox = true;
    [SerializeField] private Canvas letterBoxCanvas;

    private void Start()
    {
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
        var camera = Camera.main;

        camera.rect = new Rect(1f, 1f, 1f, 1f);
        SetLetterBoxInactive();

        var aspectRatio = (float)Screen.width / (float)Screen.height;
        var orthographicSize = targetWidth / (aspectRatio * 2f);
        camera.orthographicSize = orthographicSize;

        var cameraPositionY = bottomY + orthographicSize;
        camera.transform.position = new Vector3(camera.transform.position.x, cameraPositionY, camera.transform.position.z);
    }

    private void AdjustCameraUsingLetterBox()
    {
        var camera = Camera.main;

        float screenAspectRatio = (float)Screen.width / (float)Screen.height;
        float targetAspectRatio = targetWidth / targetHeight;

        if (screenAspectRatio >= targetAspectRatio)
        {
            // 좌우 레터박스
            float viewportWidth = targetAspectRatio / screenAspectRatio;
            camera.rect = new Rect((1f - viewportWidth) / 2f, 0f, viewportWidth, 1f);
            SetLetterBoxSize((1f - viewportWidth) / 2f, 1f);
        }
        else
        {
            // 상하 레터박스
            float viewportHeight = screenAspectRatio / targetAspectRatio;
            camera.rect = new Rect(0f, (1f - viewportHeight) / 2f, 1f, viewportHeight);
            SetLetterBoxSize(1f, (1f - viewportHeight) / 2f);
        }

        var orthographicSize = targetHeight / 2f;
        camera.orthographicSize = orthographicSize;
        var cameraPositionY = bottomY + orthographicSize;
        camera.transform.position = new Vector3(camera.transform.position.x, cameraPositionY, camera.transform.position.z);
    }

    void SetLetterBoxSize(float widthPercentage, float heightPercentage)
    {
        RectTransform canvasRectTransform = letterBoxCanvas.GetComponent<RectTransform>();
        
        float newWidth = canvasRectTransform.rect.width * widthPercentage;
        float newHeight = canvasRectTransform.rect.height * heightPercentage;

        var letterBoxes = letterBoxCanvas.GetComponentsInChildren<Image>();
        foreach (var letterBox in letterBoxes)
        {
            letterBox.gameObject.SetActive(true);
            letterBox.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }
    }

    void SetLetterBoxInactive()
    {
        var letterBoxes = letterBoxCanvas.GetComponentsInChildren<Image>();
        foreach (var letterBox in letterBoxes)
        {
            letterBox.gameObject.SetActive(false);
        }
    }
}
