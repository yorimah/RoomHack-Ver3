﻿using UnityEngine;

public class ToolEvent_Accelerate : ToolEventBase
{
    public override toolTag thisToolTag { get; set; } = toolTag.Accelerate;

    protected override void Enter()
    {
        //EventAdd();
        ToolManager.Instance.toolPlayAction += AccelerateAction;
    }

    protected override void Execute()
    {
        
    }

    protected override void Exit()
    {
        //EventRemove();
    }

    void AccelerateAction()
    {
        Debug.Log("tool使ったお");
        ToolManager.Instance.ChangeRam(1);
    }

}
