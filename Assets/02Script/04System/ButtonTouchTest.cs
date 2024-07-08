using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonTouchTest : MonoBehaviour
{
    public TouchManager touchManager;
    public Button singleTouchButton;
    public Button multiTouchButton;

    public void Start()
    {
        singleTouchButton.onClick.AddListener(OnSingleTouchButtonClicked);
        multiTouchButton.onClick.AddListener(OnMultiTouchButtonClicked);
    }

    private void OnSingleTouchButtonClicked()
    {
        touchManager.SetSingleTouch();
    }

    private void OnMultiTouchButtonClicked()
    {
        touchManager.SetMultiTouch();
    }
}