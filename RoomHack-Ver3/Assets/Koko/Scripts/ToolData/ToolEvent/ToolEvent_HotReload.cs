using UnityEngine;

public class ToolEvent_HotReload : ToolEventBase, IToolEvent_ToolManager
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.HotReload;


    // IToolEvent_ToolManager
    public ToolManager toolManager { get; set; }
    public void GetToolManager()
    {
        toolManager = ToolManager.Instance;
    }


    protected override void Enter()
    {
        GetToolManager();
        //EventAdd();
    }


    protected override void Execute()
    {
        if (toolManager.GetHandData().Count > 0)
        {
            int lowestCost = 100;
            int index = 0;
            for (int i = 0; i < toolManager.GetHandData().Count; i++)
            {
                int nowCost = toolManager.toolDataBank.toolDataList[(int)toolManager.GetHandData()[i]].toolCost;
                if (nowCost < lowestCost)
                {
                    lowestCost = nowCost;
                    index = i;
                }
            }
            toolManager.HandTrash(index);
        }

        toolManager.DeckDraw();
        toolManager.DeckDraw();

        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }



}
