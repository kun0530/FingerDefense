using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFindable
{
    GameObject FindTarget();
    List<GameObject> FindTargets();
}
