using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Bind : ToolEvent
{
    float timer = 5;

    private void Update()
    {
        timer -= GameTimer.Instance.ScaledDeltaTime;
        ToolAction();

        if (timer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override void ToolAction()
    {
        Debug.Log("対象の移動停止中");
    }
}
