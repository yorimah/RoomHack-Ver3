using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Blind : ToolEvent
{
    float timer = 5;

    Enemy targetData;
    int startRate;

    private void Start()
    {
        targetData = targetObject.GetComponent<Enemy>();
        startRate = targetData.shotRate;
    }

    private void Update()
    {
        timer -= GameTimer.Instance.ScaledDeltaTime;

        targetData.shotRate = 0;

        if (timer < 0)
        {
            targetData.shotRate = startRate;
            Destroy(this.gameObject);
        }
    }

    public override void ToolAction()
    {
    }
}
