//custom Logger 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
#if UNITY_EDITOR
    public static void Log(string message)
    {
        Debug.Log(message);
    }
    public static void Log(object message)
    {
        Debug.Log(message);
    }
    
    public static void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    public static void LogError(string message)
    {
        Debug.LogError(message);
    }
#endif
    
}