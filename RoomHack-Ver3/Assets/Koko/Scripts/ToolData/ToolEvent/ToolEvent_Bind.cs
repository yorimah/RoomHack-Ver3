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
        targetData = targetObject.GetComponent<Enemy>();
        startSpeed = targetData.moveSpeed;
    }

    private void Update()
    {
        timer -= GameTimer.Instance.ScaledDeltaTime;

        targetData.moveSpeed = 0;

        if (timer < 0)
        {
            targetData.moveSpeed = startSpeed;
            Destroy(this.gameObject);
        }
    }

    public override void ToolAction()
    {
    }
}
