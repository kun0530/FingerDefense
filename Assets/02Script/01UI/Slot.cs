using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class Slot : MonoBehaviour
{
    private TextMeshProUGUI Text;
    public Image elementImage;
    public Image skillImage;
    public Image assetImage;
    public Image gradeImage;
    
    public void Awake()
    {
        Text=GetComponentInChildren<TextMeshProUGUI>();
        
    }

    public void OnEnable()
    {
        
    }

    public void Start()
    {
        
    }
}
