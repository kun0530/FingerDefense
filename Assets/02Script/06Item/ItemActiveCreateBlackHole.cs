using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Item/Active Create BlackHole", fileName = "Item.asset")]
public class ItemActiveCreateBlackHole : ActiveItem
{
    public BlackHole blackHolePrefab;

    public float radius;

    private bool isDragging;
    private InputManager inputManager;
    private DragAndDrop dragAndDrop;

    private BlackHole activeBlackHole;

    public override void Init()
    {
        base.Init();
        isDragging = false;

        var inputSystemGo = GameObject.FindWithTag("InputManager");
        dragAndDrop = inputSystemGo?.GetComponent<DragAndDrop>();
        inputManager = inputSystemGo?.GetComponent<InputManager>();
    }

    public override void UseItem()
    {
        if (isDragging)
            return;

        isDragging = true;
        if (inputManager)
            inputManager.OnClick += PutBlackHole;
    }

    public override void CancelItem()
    {
        base.CancelItem();
        // if (activeBlackHole)
        //     Destroy(activeBlackHole.gameObject);
    }

    public override void UpdateItem()
    {
        base.UpdateItem();
    }

    private void PutBlackHole(InputAction.CallbackContext context)
    {
        isDragging = false;
        base.UseItem();
        var pos = Camera.main!.ScreenToWorldPoint(dragAndDrop.GetPointerPosition());
        pos.z = 0;
        activeBlackHole = GameObject.Instantiate(blackHolePrefab, pos, Quaternion.identity);
        activeBlackHole.LifeTime = duration;
        if (inputManager)
            inputManager.OnClick -= PutBlackHole;
    }
}
