using UnityEngine;

public class ToolEvent_Blink : ToolEventBase
{


    public override ToolTag thisToolTag { get; set; } = ToolTag.none;

    protected override void Enter()
    {
        //EventAdd(hackTargetObject);
        Debug.Log("このToolEvent_Blinkはまだ効果ないに");
        //if (Player.Instance.nowSpecialAction != Player.SpecialAction.Blink)
        //{
        //    Player.Instance.nowSpecialAction = Player.SpecialAction.Blink;
        //    Player.Instance.specialActionCount = 0;
        //}

        //Player.Instance.specialActionCount += 1;
    }

    protected override void Execute()
    {
        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove(hackTargetObject);
    }
}
