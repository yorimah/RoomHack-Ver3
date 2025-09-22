using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_OverHeat : ToolEvent
{
    float timer = 5;

    float damageTimer = 1;
    float damage = 10;

    private void Update()
    {
        damageTimer -= GameTimer.Instance.ScaledDeltaTime;
        if (damageTimer <= 0)
        {
            ToolAction();
            damageTimer = 1;
        }


        timer -= GameTimer.Instance.ScaledDeltaTime;

        if (timer < 0)
        {
            Destroy(this);
        }
    }

    public override void ToolAction()
    {
        Debug.Log("対象は" + damage + "ダメージ！");
    }
}
