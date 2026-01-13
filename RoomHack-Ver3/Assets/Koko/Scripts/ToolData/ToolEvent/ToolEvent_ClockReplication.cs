using UnityEngine;
using System.Collections.Generic;

public class ToolEvent_ClockReplication : ToolEventBase, IToolEvent_Target
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.ClockReplication;


    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }

    List<ToolEventBase> nowEvent;

    protected override void Enter()
    {

        EffectManager.Instance.ActEffect(EffectManager.EffectType.HitDie, hackTargetObject.transform.position);


        //EventAdd();
    }

    protected override void Execute()
    {
        // 対象についてるHackEventを取得
        nowEvent = hackTargetObject.GetComponent<IHackObject>().nowHackEvent;

        // HackEvent内のIToolEvent_Timeを検知し、そのtimerを2倍にする
        for (int i = 0; i < nowEvent.Count; i++)
        {
            if (nowEvent[i].TryGetComponent<IToolEvent_Time>(out var timeEvent))
            {
                timeEvent.timer *= 2;
            }
        }

        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }


}
