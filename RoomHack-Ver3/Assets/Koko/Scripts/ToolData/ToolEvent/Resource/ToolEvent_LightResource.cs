using UnityEngine;

public class ToolEvent_LightResource : ToolEventBase, IToolEvent_ToolManager
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.LightResource;


    // ToolManager
    public ToolManager toolManager { get; set; }
    public void GetToolManager()
    {
        toolManager = ToolManager.Instance;
    }


    protected override void Enter()
    {
        //EventAdd();
        GetToolManager();
    }

    protected override void Execute()
    {
        // ２どろー
        toolManager.DeckDraw();
        toolManager.DeckDraw();
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
