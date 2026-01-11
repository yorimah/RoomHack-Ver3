
public class ToolEvent_None : ToolEventBase
{
    public override ToolTag thisToolTag { get;  set; } = ToolTag.none;

    protected override void Enter()
    {
        //EventAdd();
    }

    protected override void Execute()
    {
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }
}
