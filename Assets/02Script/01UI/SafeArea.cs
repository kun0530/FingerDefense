using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform safeArea;
    
    private void Awake()
    {
        safeArea = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ApplySafeArea();
    }

    private void Update()
    {
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        var minAnchor = Screen.safeArea.min;
        var maxAnchor = Screen.safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        safeArea.anchorMin = minAnchor;
        safeArea.anchorMax = maxAnchor;
    }
}