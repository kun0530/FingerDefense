using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Start()
    {
        var main = Camera.main;
        var targetAspect = 16.0f / 9.0f; // 목표 종횡비
        var windowAspect = (float)Screen.width / (float)Screen.height; // 현재 화면 종횡비
        var scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            if (main != null)
            {
                var rect = main.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                main.rect = rect;
            }
        }
        else 
        {
            var scaleWidth = 1.0f / scaleHeight;

            if (main != null)
            {
                var rect = main.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                main.rect = rect;
            }
        }
    }

    private void OnPreCull() => GL.Clear(true, true, Color.black);
}