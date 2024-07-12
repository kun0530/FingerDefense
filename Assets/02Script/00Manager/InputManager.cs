using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    #region Events
    public event Action<InputAction.CallbackContext> OnClick;
    public event Action<InputAction.CallbackContext> OnRelease;
    public event Action<InputAction.CallbackContext> OnDrag;
    public event Action<InputAction.CallbackContext> OnBack; 
    #endregion
    private Control control;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        control = new Control();
    }
    
    
    private void OnEnable()
    {
        control.Enable();
        control.MonsterDrag.Click.performed += context => OnClick?.Invoke(context);
        control.MonsterDrag.Release.performed += context => OnRelease?.Invoke(context);
        control.MonsterDrag.Drag.performed += context => OnDrag?.Invoke(context);
        control.UI.Back.performed += context => OnBack?.Invoke(context);
    }
    
    private void OnDisable()
    {
        control.Disable();

        control.UI.Back.performed -= context => OnBack?.Invoke(context);
        control.MonsterDrag.Click.performed -= context => OnClick?.Invoke(context);
        control.MonsterDrag.Release.performed -= context => OnRelease?.Invoke(context);
        control.MonsterDrag.Drag.performed -= context => OnDrag?.Invoke(context);
        
    }
    
    

}
