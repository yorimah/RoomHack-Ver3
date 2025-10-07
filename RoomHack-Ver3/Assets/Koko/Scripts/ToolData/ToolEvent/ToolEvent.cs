using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolEvent : MonoBehaviour
{
    public GameObject targetObject;

    public virtual void Tracking()
    {
        this.transform.position = targetObject.transform.position;
        this.transform.localEulerAngles = targetObject.transform.localEulerAngles;
    }

    public abstract void ToolAction();
}
