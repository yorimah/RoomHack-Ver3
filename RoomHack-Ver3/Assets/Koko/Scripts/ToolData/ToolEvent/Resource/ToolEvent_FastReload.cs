using UnityEngine;

public class ToolEvent_FastReload : ToolEventBase
{

    public override toolTag thisToolTag { get; set; } = toolTag.FastReload;

    protected override void Enter()
    {
        //EventAdd();
    }

    protected override void Execute()
    {
        ToolManager toolManager = ToolManager.Instance;
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
