using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameCameraController : MonoBehaviour
{
    public enum ScreenMode
    {
        FULL_SCREEN_FIXED_WIDTH,
        ASPECT_RATIO,
        SAFE_AREA_FIXED_WIDTH
    }
    private Camera mainCamera;

    [Header("스크린 모드")]
    [SerializeField] private ScreenMode defaultLandScapeMode; // 가로
    [SerializeField] private ScreenMode defaultPortraitMode; // 세로
    [SerializeField] private ScreenMode currentScreenMode;
    private ScreenMode prevScreenMode;
    public bool IsScreenModeChange
    {
        get => prevScreenMode != currentScreenMode;
    }

    [Header("타켓 값")]
    public float targetWidth = 20f;
    [SerializeField] private float targetHeight = 20f;
    public float bottomY;

    [Header("몬스터 드래그 줌 아웃 / 줌 인")]
    [SerializeField] private float zoomOutWidth = 25f;
    [SerializeField] private float zoomTime = 0.25f;

    [Header("레터박스")]
    [SerializeField] private Color letterBoxColor;

    [Header("UI 카메라")]
    [SerializeField] private List<Camera> uiCameras;

    public float currentWidth { get; private set; }

    private float startWidth;
    private float endWidth;
    private float changeDuration;
    private float timer = 0f;
    private bool isWidthChange = false;

    private float currentScreenWidth;
    private float currentScreenHeight;
    public bool IsResolutionChange
    {
        get => currentScreenHeight != Screen.height
            || currentScreenWidth != Screen.width;
    }

    public event Action onScreenChange;

    private void Awake()
    {
        mainCamera = Camera.main;
        currentWidth = targetWidth;
        
        prevScreenMode = currentScreenMode;
        currentScreenHeight = Screen.height;
        currentScreenWidth = Screen.width;

        ChangeResolution();
    }

    void OnEnable()
    {
#if !UNITY_EDITOR
        RenderPipelineManager.beginFrameRendering += OnEndCameraRendering;
        // RenderPipelineManager.beginContextRendering += OnEndCameraRendering;
        // RenderPipelineManager.beginCameraRendering
#endif
    }
    void OnDisable()
    {
#if !UNITY_EDITOR
        RenderPipelineManager.beginFrameRendering -= OnEndCameraRendering;
#endif
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera[] camera)
    {
        GL.Clear(true, true, letterBoxColor);
    }

    // private void OnPreCull()
    // {
    //     Logger.Log("OnPreCull");
    // }

    // private void OnPreRender()
    // {
    //     Logger.Log("OnPreRender");
    // }

    // private void OnPostRender()
    // {
    //     Logger.Log("OnPostRender");
    // }

    private void Update()
    {
        if (isWidthChange)
        {
            ChangeTargetWidth();
        }

        if (IsResolutionChange || IsScreenModeChange)
        {
            ChangeResolution();
            onScreenChange?.Invoke();
        }
    }

    private void LateUpdate()
    {
        prevScreenMode = currentScreenMode;
        currentScreenHeight = Screen.height;
        currentScreenWidth = Screen.width;
    }

    private void ChangeResolution()
    {
        if (IsResolutionChange)
        {
            if (Screen.height > Screen.width) // 강제 세로모드에 대한 대응
            {
                currentScreenMode = defaultPortraitMode;
            }
            else // 가로모드에 대한 대응
            {
                currentScreenMode = defaultLandScapeMode;
            }
        }

        switch (currentScreenMode)
        {
            case ScreenMode.FULL_SCREEN_FIXED_WIDTH:
                AdjustCamera();
                break;
            case ScreenMode.ASPECT_RATIO:
                AdjustCameraUsingLetterBox();
                break;
            case ScreenMode.SAFE_AREA_FIXED_WIDTH:
                AdjustCameraForSafeArea();
                break;
        }

        ResizeAndRepositionCamera();
        AdjustUiCamera();
    }

    private void AdjustCamera()
    {
        var normalRect = new Rect(0f, 0f, 1f, 1f);
        mainCamera.rect = normalRect;

        // var aspectRatio = (float)Screen.width / (float)Screen.height;
        // var orthographicSize = currentWidth / (aspectRatio * 2f);
        // mainCamera.orthographicSize = orthographicSize;

        // var cameraPositionY = bottomY + orthographicSize;
        // mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraPositionY, mainCamera.transform.position.z);
    }

    private void AdjustCameraForSafeArea()
    {
        var safeAreaRect = Screen.safeArea;
        var cameraRect = safeAreaRect;
        cameraRect.x /= Screen.width;
        cameraRect.y /= Screen.height;
        cameraRect.width /= Screen.width;
        cameraRect.height /= Screen.height;

        mainCamera.rect = cameraRect;

        // var aspectRatio = (float)safeAreaRect.width / (float)safeAreaRect.height;
        // var orthographicSize = currentWidth / (aspectRatio * 2f);
        // mainCamera.orthographicSize = orthographicSize;

        // var cameraPositionY = bottomY + orthographicSize;
        // mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraPositionY, mainCamera.transform.position.z);
    }

    private void AdjustCameraUsingLetterBox()
    {
        float screenAspectRatio = (float)Screen.width / (float)Screen.height;
        float targetAspectRatio = currentWidth / targetHeight;

        if (screenAspectRatio >= targetAspectRatio)
        {
            // 좌우 레터박스
            float viewportWidth = targetAspectRatio / screenAspectRatio;
            var adjustedRect = new Rect((1f - viewportWidth) / 2f, 0f, viewportWidth, 1f);
            mainCamera.rect = adjustedRect;
        }
        else
        {
            // 상하 레터박스
            float viewportHeight = screenAspectRatio / targetAspectRatio;
            var adjustedRect = new Rect(0f, (1f - viewportHeight) / 2f, 1f, viewportHeight);
            mainCamera.rect = adjustedRect;
        }

        // var orthographicSize = targetHeight / 2f;
        // mainCamera.orthographicSize = orthographicSize;
        // var cameraPositionY = bottomY + orthographicSize;
        // mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraPositionY, mainCamera.transform.position.z);
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

    public void ZoomOutCamera()
    {
        SetTargetWidth(zoomOutWidth, zoomTime);
    }

    public void ResetCamera()
    {
        SetTargetWidth(targetWidth, zoomTime);
    }

    public void SetTargetWidth(float target, float duration)
    {
        if (target < 0f || duration < 0f)
            return;

        startWidth = currentWidth;
        endWidth = target;
        changeDuration = duration;
        timer = 0f;

        isWidthChange = true;
    }

    private void ChangeTargetWidth()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < changeDuration)
        {
            currentWidth = Mathf.Lerp(startWidth, endWidth, timer / changeDuration);
        }
        else
        {
            currentWidth = endWidth;
            isWidthChange = false;
        }

        ResizeAndRepositionCamera();
    }

    private void ResizeAndRepositionCamera()
    {
        var aspectRatio = mainCamera.aspect;
        var orthographicSize = currentWidth / (aspectRatio * 2f);
        mainCamera.orthographicSize = orthographicSize;
        var cameraPositionY = bottomY + orthographicSize;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraPositionY, mainCamera.transform.position.z);
    }

    private void AdjustUiCamera()
    {
        foreach (var uiCamera in uiCameras)
        {
            uiCamera.rect = mainCamera.rect;
            uiCamera.orthographicSize = mainCamera.orthographicSize;
        }
    }
}