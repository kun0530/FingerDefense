using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxArea : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        CameraController.SetLetterBoxRect(rect);
    }

    private void Update()
    {
        CameraController.SetLetterBoxRect(rect);
    }
}
