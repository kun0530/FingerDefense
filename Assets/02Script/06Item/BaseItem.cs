using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseItem : ScriptableObject
{
    private StageManager stageManager;
    protected StageManager StageManager
    {
        get
        {
            if (!stageManager)
            {
                stageManager = GameObject.FindWithTag(Defines.Tags.STAGE_MANAGER_TAG)?.GetComponent<StageManager>();
            }

            return stageManager;
        }
    }

    [HideInInspector] public Button button;

    public virtual bool IsPassive { get => false; }

    public abstract bool UseItem();
    public virtual bool CancelItem() { return true; }
    public virtual void UpdateItem() { }
}