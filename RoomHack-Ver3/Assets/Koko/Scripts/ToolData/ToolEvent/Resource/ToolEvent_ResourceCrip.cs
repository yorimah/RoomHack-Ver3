using UnityEngine;

public class ToolEvent_ResourceCrip : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.none;

    protected override void Enter()
    {
        EventAdd();
    }

    protected override void Execute()
    {
        ToolManager.Instance.ToolDraw(1);
        EventEnd();
    }

    protected override void Exit()
    {
        EventRemove();
    }
}
