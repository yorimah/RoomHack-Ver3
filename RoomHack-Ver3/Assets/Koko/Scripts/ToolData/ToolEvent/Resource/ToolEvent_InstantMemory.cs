using UnityEngine;

public class ToolEvent_InstantMemory : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.InstantMemory;

    protected override void Enter()
    {
        EventAdd();
    }

    protected override void Execute()
    {
        ToolManager.Instance.RamAdd(3);
        EventEnd();
    }

    protected override void Exit()
    {
        EventRemove();
    }
}
