using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Blind : ToolEvent
{
    float timer = 5;

    Enemy targetData;
    float startInterval;

    private void Start()
    {
        EventAdd();

        EffectManager.Instance.EffectAct(EffectManager.EffectType.Bad, this.gameObject, 5);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startInterval = targetData.shotIntervalTime;
    }

    private void Update()
    {
        targetData.shotIntervalTime = 9999;

        Tracking();

        timer -= GameTimer.Instance.ScaledDeltaTime;
        if (timer < 0)
        {
            targetData.shotIntervalTime = startInterval;

            EventRemove();
        }
    }

    public override void ToolAction()
    {
    }
}
