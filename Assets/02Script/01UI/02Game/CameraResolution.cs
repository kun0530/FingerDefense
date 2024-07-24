using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Start()
    {
        var camera = GetComponent<Camera>();
        var targetaspect = 16.0f / 9.0f; // 목표 종횡비
        var windowaspect = (float)Screen.width / (float)Screen.height; // 현재 화면 종횡비
        var scaleheight = windowaspect / targetaspect;

        if (scaleheight < 1.0f)
        {  
            var rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else 
        {
            var scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    private void OnPreCull() => GL.Clear(true, true, Color.black);
}