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

        targetData = hackTargetObject.GetComponent<Enemy>();
        startInterval = targetData.shotIntervalTime;
    }

    private void Update()
    {
        timer -= GameTimer.Instance.ScaledDeltaTime;

        targetData.shotIntervalTime = 9999;

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
