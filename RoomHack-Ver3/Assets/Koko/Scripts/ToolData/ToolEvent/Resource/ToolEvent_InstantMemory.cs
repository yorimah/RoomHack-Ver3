using UnityEngine;

public class ToolEvent_InstantMemory : ToolEventBase
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.InstantMemory;

    [SerializeField, Header("Ram回復量")]
    int AddRam = 3;

    protected override void Enter()
    {
        //EventAdd();
    }

    protected override void Execute()
    {
        ToolManager.Instance.ChangeRam(AddRam);
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
