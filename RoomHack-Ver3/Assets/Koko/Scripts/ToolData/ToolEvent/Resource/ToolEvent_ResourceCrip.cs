using UnityEngine;

public class ToolEvent_ResourceCrip : ToolEventBase
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.none;

    protected override void Enter()
    {
        //EventAdd();
    }

    protected override void Execute()
    {
        ToolManager.Instance.DeckDraw();
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
