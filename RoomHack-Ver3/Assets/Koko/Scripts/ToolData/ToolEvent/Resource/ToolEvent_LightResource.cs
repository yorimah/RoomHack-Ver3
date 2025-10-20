using UnityEngine;

public class ToolEvent_LightResource : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.LightResource;

    protected override void Enter()
    {
        EventAdd();
    }

    protected override void Execute()
    {
        ToolManager.Instance.ToolDraw(2);
        EventEnd();
    }

    protected override void Exit()
    {
        EventRemove();
    }
}
