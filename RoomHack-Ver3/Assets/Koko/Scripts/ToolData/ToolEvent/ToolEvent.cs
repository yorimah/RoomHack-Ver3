using UnityEngine;

public abstract class ToolEvent : MonoBehaviour
{
    [HideInInspector]
    public abstract toolTag thisToolTag { get; set; }

    public GameObject hackTargetObject;
    public bool isEventAct = false;

    public virtual void EventAdd()
    {
        hackTargetObject.GetComponent<IHackObject>().nowHackEvent.Add(this);
    }

    public virtual void EventRemove()
    {
        hackTargetObject.GetComponent<IHackObject>().nowHackEvent.Remove(this);
    }

    public virtual void Tracking()
    {
        this.transform.position = hackTargetObject.transform.position;
        this.transform.localEulerAngles = hackTargetObject.transform.localEulerAngles;
    }

    public abstract void ToolAction();
}
