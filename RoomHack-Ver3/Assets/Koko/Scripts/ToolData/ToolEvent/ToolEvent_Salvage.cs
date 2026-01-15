using UnityEngine;

public class ToolEvent_Salvage : ToolEventBase, IToolEvent_ToolManager
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.Salvage;


    // IToolEvent_ToolManager
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
        if (toolManager.GetTrashData().Count > 0)
        {
            int rand = Random.Range(0, toolManager.GetTrashData().Count);
            toolManager.TrashToHand(rand);
        }
        else
        {
            Debug.Log("無をサルベージ");
        }

        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
