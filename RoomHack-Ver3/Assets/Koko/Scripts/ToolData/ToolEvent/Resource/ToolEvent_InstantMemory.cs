using UnityEngine;

public class ToolEvent_InstantMemory : ToolEventBase, IToolEvent_ToolManager
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.InstantMemory;


    // ToolManager
    public ToolManager toolManager { get; set; }
    public void GetToolManager()
    {
        toolManager = ToolManager.Instance;
    }


    [SerializeField, Header("Ram回復量")]
    int AddRam = 3;

    protected override void Enter()
    {
        //EventAdd();
        GetToolManager();
    }

    protected override void Execute()
    {
        toolManager.ChangeRam(AddRam);
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
