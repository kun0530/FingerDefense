using System;
using UnityEngine;

public class MaintainAspectRatio : MonoBehaviour
{
    private Camera cam;
    public float fixedWidth = 10f; // 모든 기기에서 동일하게 유지할 고정 가로 길이

    private void Start()
    {
        cam = GetComponent<Camera>();
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        // 1. 모바일 기기의 해상도를 가져옵니다.
        float deviceWidth = (float)Screen.width;
        float deviceHeight = (float)Screen.height;
        float deviceAspectRatio = deviceWidth / deviceHeight;

        // 2. 고정 가로 길이에 따라 카메라의 Orthographic Size를 조정합니다.
        float targetHeight = fixedWidth / deviceAspectRatio;
        cam.orthographicSize = (targetHeight / 2) * 3;

        // 3. 기기의 가로 길이가 고정 길이보다 작은 경우, Viewport를 조정합니다.
        if (deviceWidth < fixedWidth)
        {
            float scaleFactor = fixedWidth / deviceWidth;
            cam.orthographicSize *= scaleFactor;

            // 4. 카메라의 Viewport를 조정하여 화면을 꽉 채웁니다.
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = 1.0f / scaleFactor;
            rect.x = 0;
            rect.y = (1.0f - rect.height) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // Viewport를 기본값으로 설정합니다.
            cam.rect = new Rect(0, 0, 1, 1);
        }
    }
}