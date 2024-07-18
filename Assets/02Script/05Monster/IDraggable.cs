using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraggable
{
    bool IsDraggable { get; }
    bool TryDrag();
    bool TryFall();
}