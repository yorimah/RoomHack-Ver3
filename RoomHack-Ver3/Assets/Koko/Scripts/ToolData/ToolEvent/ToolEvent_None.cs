using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_None : ToolEventBase
{
    public override toolTag thisToolTag { get;  set; } = toolTag.none;

    protected override void Enter()
    {
        //EventAdd();
    }

    protected override void Execute()
    {
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
