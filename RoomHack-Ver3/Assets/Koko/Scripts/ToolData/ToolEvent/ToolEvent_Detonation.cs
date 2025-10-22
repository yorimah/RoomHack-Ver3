using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Detonation : ToolEventBase
{
    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }


    public override toolTag thisToolTag { get; set; } = toolTag.Detonation;

    [SerializeField]
    GameObject granadePrefab;

    GameObject granade;

    protected override void Enter()
    {
        EventAdd(hackTargetObject);
        granade = Instantiate(granadePrefab, this.transform);
    }

    protected override void Execute()
    {
        Tracking(hackTargetObject);


        if (granade == null)
        {
            EventEnd();
        }
    }

    protected override void Exit()
    {
        EventRemove(hackTargetObject);
    }

    //public override void ToolAction()
    //{
    //}
}
