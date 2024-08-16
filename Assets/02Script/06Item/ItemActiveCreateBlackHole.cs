using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Item/Active Create BlackHole", fileName = "Item.asset")]
public class ItemActiveCreateBlackHole : ActiveItem
{
    [Header("블랙홀 프리팹")]
    public BlackHole blackHolePrefab;

    public float scale;
    public Rect blackHoleRange;

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
        if (inputManager)
            inputManager.OnClick -= PutBlackHole;

        var pos = Camera.main!.ScreenToWorldPoint(dragAndDrop.GetPointerPosition());
        if (pos.x < blackHoleRange.x || pos.x > blackHoleRange.x + blackHoleRange.width
            || pos.y < blackHoleRange.y || pos.y > blackHoleRange.y + blackHoleRange.height)
            return;

        pos.y = blackHoleRange.y + blackHoleRange.height / 2f;
        pos.z = pos.y;
        activeBlackHole = GameObject.Instantiate(blackHolePrefab, pos, Quaternion.identity);
        activeBlackHole.transform.localScale = Vector3.one * scale;
        activeBlackHole.LifeTime = duration;

        base.UseItem();
    }
}
