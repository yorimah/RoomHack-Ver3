using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_CCTVHack : ToolEvent
{
    private void Start()
    {
        ToolAction();
    }

    public override void ToolAction()
    {
        Debug.Log("かめらはっきんぐしたおー");
        Destroy(this.gameObject);
    }
}
