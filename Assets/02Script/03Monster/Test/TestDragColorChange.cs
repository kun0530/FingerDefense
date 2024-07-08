using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDragColorChange : IDraggable
{
    private SpriteRenderer ren;
    private bool isRenExist = false;

    public TestDragColorChange(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<SpriteRenderer>(out ren))
        {
            isRenExist = true;
        }
    }

    public void DragEnter()
    {
        if (isRenExist)
            ren.material.color = Color.red;
    }

    public void DragExit()
    {
        if (isRenExist)
            ren.material.color = Color.blue;
    }

    public void DragUpdate()
    {
        
    }
}