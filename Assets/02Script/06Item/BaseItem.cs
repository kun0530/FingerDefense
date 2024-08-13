using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseItem : ScriptableObject
{
    [HideInInspector] public int id;

    private StageManager stageManager;
    protected StageManager StageMgr
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

    [HideInInspector] public UiSlotButton button;
    [HideInInspector] public int count;

    public virtual bool IsPassive { get => true; }

    public virtual void Init()
    {
        stageManager = null;
        button = null;
        count = 0;
    }

    public abstract void UseItem();
    public virtual void CancelItem() { }
    public virtual void UpdateItem() { }
}