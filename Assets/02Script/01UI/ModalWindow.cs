using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindow : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI buttonText;
    public Button okButton;
    
    public void SetHeader(string title)
    {
        titleText.text = title;
    }
    public void SetBody(string content)
    {
        contentText.text = content;
    }
    
    public void SetButton(string button, UnityEngine.Events.UnityAction action)
    {
        buttonText.text = button;
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(action);
        okButton.onClick.AddListener(Close);
#if UNITY_EDITOR
        if (action == null)
        {
            Debug.LogWarning("Button action is null");
        }
        Debug.Log("Button action set: " + button);
#endif
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
    
    public static ModalWindow Create(GameObject modalPrefab, GameObject canvas)
    {
        var modalObject = Instantiate(modalPrefab, canvas.transform);
        return modalObject.GetComponent<ModalWindow>();
    }
}
