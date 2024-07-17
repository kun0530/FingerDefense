using UnityEngine;

public static class TestDragFactory
{
    public static IDraggable GenerateDragBehavior(string dragBehaviorName, GameObject gameObject)
    {
        switch(dragBehaviorName)
        {
            case "Color1":
                return new TestDragColorChange(gameObject);
            case "Color2":
                return new TestDragColorChange2(gameObject);
            default:
                return new TestDragColorChange(gameObject);
        }
    }
}
