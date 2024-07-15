using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaintainAspectRatio : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        
    }

    private void Start()
    {
        //현재 해상도의 가로 세로 비율을 구한다.
        float screenRatio = (float)Screen.width / (float)Screen.height;
        //게임의 가로 세로 비율을 구한다.
        float targetRatio = 16f / 9f;
    }
}