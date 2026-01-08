using UnityEngine;

public class ToolEvent_FastReload : ToolEventBase, IToolEvent_ToolManager
{

    public override ToolTag thisToolTag { get; set; } = ToolTag.FastReload;


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
        int handNum = toolManager.GetHandData().Count;
        for (int i = 0; i < handNum; i++)
        {
            toolManager.HandTrash(0);
        }
        for (int i = 0; i < handNum; i++)
        {
            toolManager.DeckDraw();
        }

        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
