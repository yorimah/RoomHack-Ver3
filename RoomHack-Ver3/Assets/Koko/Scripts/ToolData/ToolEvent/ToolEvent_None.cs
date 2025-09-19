using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_None : ToolEvent
{
    private void Start()
    {
        ToolAction();
    }

    public override void ToolAction()
    {
        Debug.Log("Tool0番参照、ばぐです");
        Destroy(this.gameObject);
    }
}
