using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Bind : ToolEvent
{
    float timer = 5;

    Enemy targetData;
    float startSpeed;

    private void Start()
    {
        EventAdd();

        EffectManager.Instance.EffectAct(EffectManager.EffectType.Bad, this.gameObject, 5);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startSpeed = targetData.moveSpeed;
    }

    private void Update()
    {
        targetData.moveSpeed = 0;
        
        Tracking();

        timer -= GameTimer.Instance.ScaledDeltaTime;
        if (timer < 0)
        {
            targetData.moveSpeed = startSpeed;
            EventRemove();
        }
    }

    public override void ToolAction()
    {
    }
}
