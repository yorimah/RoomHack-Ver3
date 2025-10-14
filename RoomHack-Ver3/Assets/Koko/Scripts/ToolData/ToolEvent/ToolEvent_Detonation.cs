using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_Detonation : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.Detonation;

    [SerializeField]
    GameObject granadePrefab;

    GameObject granade;

    bool isSet = false;

    private void Update()
    {
        if (isEventAct)
        {
            if (!isSet)
            {
                EventAdd();
                granade = Instantiate(granadePrefab, this.transform);
                isSet = true;
            }


            Tracking();


            if (granade == null)
            {
                EventRemove();

                isSet = false;
                isEventAct = false;
            }


        }
    }

    public override void ToolAction()
    {
    }
}
