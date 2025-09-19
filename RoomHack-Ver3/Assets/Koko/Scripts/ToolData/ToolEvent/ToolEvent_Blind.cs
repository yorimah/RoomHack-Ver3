using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Blind : ToolEvent
{
    float timer = 5;

    private void Update()
    {
        timer -= GameTimer.Instance.ScaledDeltaTime;
        ToolAction();

        if (timer < 0)
        {
            Destroy(this);
        }
    }

    public override void ToolAction()
    {
        Debug.Log("対象の攻撃停止中");
    }
}
