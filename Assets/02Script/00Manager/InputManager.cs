using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{

    #region Events
    public event Action<InputAction.CallbackContext> OnClick;
    public event Action<InputAction.CallbackContext> OnRelease;
    public event Action<InputAction.CallbackContext> OnDrag;
    public event Action<InputAction.CallbackContext> OnBack; 
    #endregion
    private Control control;

    public GameObject QuitPanel;
    
    private void Awake()
    {
        control = new Control();
    }
    
    private void OnEnable()
    {
        control.Enable();
        control.MonsterDrag.Click.performed += OnClickHandler;
        control.MonsterDrag.Release.performed += OnReleaseHandler;
        control.MonsterDrag.Drag.performed += OnDragHandler;
        control.UI.Back.performed += OnBackHandler;
    }
    
    private void OnDisable()
    {
        control.Disable();

        control.UI.Back.performed -= OnBackHandler;
        control.MonsterDrag.Click.performed -= OnClickHandler;
        control.MonsterDrag.Release.performed -= OnReleaseHandler;
        control.MonsterDrag.Drag.performed -= OnDragHandler;
        
    }
    private void OnClickHandler(InputAction.CallbackContext context)
    {
        OnClick?.Invoke(context);
    }

    private void OnReleaseHandler(InputAction.CallbackContext context)
    {
        OnRelease?.Invoke(context);
    }

    private void OnDragHandler(InputAction.CallbackContext context)
    {
        OnDrag?.Invoke(context);
    }
    
    
    private void OnBackHandler(InputAction.CallbackContext context)
    {
        OnBack?.Invoke(context);
        QuitPanel.SetActive(!QuitPanel.activeSelf);
        Time.timeScale = QuitPanel.activeSelf ? 0 : 1;
    }
    
    
}
