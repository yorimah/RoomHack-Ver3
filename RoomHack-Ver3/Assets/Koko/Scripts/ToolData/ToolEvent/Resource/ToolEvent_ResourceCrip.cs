using UnityEngine;

public class ToolEvent_ResourceCrip : ToolEventBase, IToolEvent_ToolManager
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.none;


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
        toolManager.DeckDraw();
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
