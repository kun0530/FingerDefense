using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    private bool isSetTarget = false;

    private GameObject target;
    public GameObject Target
    {
        get => target;
        set
        {
            if (value == null)
                return;

            isSetTarget = true;
            target = value;
        }
    }

    private void Update()
    {
        if (!isSetTarget)
            return;
        
        if (target == null || !target.activeSelf)
            Destroy(gameObject);
        else
            transform.position = target.transform.position;
    }
}
