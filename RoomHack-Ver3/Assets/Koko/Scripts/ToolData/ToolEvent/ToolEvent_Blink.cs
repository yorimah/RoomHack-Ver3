using UnityEngine;

public class ToolEvent_Blink : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.Blink;

    protected override void Enter()
    {
        EventAdd();

        if (Player.Instance.nowSpecialAction != Player.SpecialAction.Blink)
        {
            Player.Instance.nowSpecialAction = Player.SpecialAction.Blink;
            Player.Instance.specialActionCount = 0;
        }

        Player.Instance.specialActionCount += 1;
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
