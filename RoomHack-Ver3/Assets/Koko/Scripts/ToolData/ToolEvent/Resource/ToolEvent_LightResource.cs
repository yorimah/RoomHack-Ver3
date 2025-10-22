using UnityEngine;

public class ToolEvent_LightResource : ToolEventBase
{
    public override toolTag thisToolTag { get; set; } = toolTag.LightResource;

    protected override void Enter()
    {
        //EventAdd();
    }

    protected override void Execute()
    {
        // ２どろー
        ToolManager.Instance.DeckDraw();
        ToolManager.Instance.DeckDraw();
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
