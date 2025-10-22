using UnityEngine;

public class ToolEvent_InstantMemory : ToolEventBase
{
    public override toolTag thisToolTag { get; set; } = toolTag.InstantMemory;

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
