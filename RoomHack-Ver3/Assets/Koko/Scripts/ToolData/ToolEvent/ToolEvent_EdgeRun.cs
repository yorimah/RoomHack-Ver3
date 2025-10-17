using UnityEngine;

public class ToolEvent_EdgeRun : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.EdgeRun;

    protected override void Enter()
    {
        EventAdd();

        if (Player.Instance.nowSpecialAction != Player.SpecialAction.EdgeRun)
        {
            Player.Instance.nowSpecialAction = Player.SpecialAction.EdgeRun;
            Player.Instance.specialActionCount = 0;
        }

        Player.Instance.specialActionCount += 3;
    }

    protected override void Execute()
    {
        EventEnd();
    }

    protected override void Exit()
    {
        EventRemove();
    }
}
