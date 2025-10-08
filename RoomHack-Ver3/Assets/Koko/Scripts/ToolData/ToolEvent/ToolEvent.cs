using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolEvent : MonoBehaviour
{
    public GameObject hackTargetObject;

    public virtual void Tracking()
    {
        this.transform.position = hackTargetObject.transform.position;
        this.transform.localEulerAngles = hackTargetObject.transform.localEulerAngles;
    }

    public abstract void ToolAction();
}
