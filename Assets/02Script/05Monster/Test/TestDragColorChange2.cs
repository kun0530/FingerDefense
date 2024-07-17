using UnityEngine;

public class TestDragColorChange2 : IDraggable
{
    private SpriteRenderer ren;
    private bool isRenExist = false;

    public TestDragColorChange2(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<SpriteRenderer>(out ren))
        {
            isRenExist = true;
        }
    }

    public void DragEnter()
    {
        if (isRenExist)
            ren.material.color = Color.green;
    }

    public void DragExit()
    {
        if (isRenExist)
            ren.material.color = Color.black;
    }

    public void DragUpdate()
    {
        
    }
}