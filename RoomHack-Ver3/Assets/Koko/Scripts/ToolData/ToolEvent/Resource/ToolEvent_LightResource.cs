using UnityEngine;

public class ToolEvent_LightResource : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.none;

    protected override void Enter()
    {
        EventAdd();
    }

    protected override void Execute()
    {

        EventEnd();
    }

    protected override void Exit()
    {
        EventRemove();
    }
}
