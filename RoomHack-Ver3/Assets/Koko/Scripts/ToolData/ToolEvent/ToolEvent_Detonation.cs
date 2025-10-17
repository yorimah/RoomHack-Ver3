using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Detonation : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.Detonation;

    [SerializeField]
    GameObject granadePrefab;

    GameObject granade;

    protected override void Enter()
    {
        EventAdd();
        granade = Instantiate(granadePrefab, this.transform);
    }

    protected override void Execute()
    {
        Tracking();


        if (granade == null)
        {
            EventEnd();
        }
    }

    protected override void Exit()
    {
        EventRemove();
    }

    //public override void ToolAction()
    //{
    //}
}
