using UnityEngine;

public class ToolEvent_Armorust : ToolEventBase, IToolEvent_Target, IToolEvent_Time
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.Armorust;


    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }

    // IToolEvent_Time
    public float setTime { get; set; } = 3;
    public float timer { get; set; } = 0;


    protected override void Enter()
    {
        EventAdd(hackTargetObject);
    }

    protected override void Execute()
    {
        EventEnd();
    }

    protected override void Exit()
    {
        EventRemove(hackTargetObject);
    }
}
