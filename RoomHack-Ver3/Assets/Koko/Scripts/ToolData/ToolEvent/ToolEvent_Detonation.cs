using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Detonation : ToolEvent
{
    [SerializeField]
    GameObject GranadePrefab;

    private void Start()
    {
        ToolAction();
    }

    public override void ToolAction()
    {
        Debug.Log("グレネード設置！");
        Destroy(this.gameObject);
    }
}
