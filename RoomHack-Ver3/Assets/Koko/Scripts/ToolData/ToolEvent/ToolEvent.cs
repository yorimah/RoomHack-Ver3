using UnityEngine;

public abstract class ToolEvent : MonoBehaviour
{
    [HideInInspector]
    public abstract toolTag thisToolTag { get; set; }

    public GameObject hackTargetObject;
    public bool isEventAct { get; private set; } = false;
    private bool isSet = false;

    public void EventStart()
    {
        isEventAct = true;
    }

    protected void EventEnd()
    {
        isEventAct = false;
    }

    protected void EventAdd()
    {
        hackTargetObject.GetComponent<IHackObject>().nowHackEvent.Add(this);
    }

    protected void EventRemove()
    {
        hackTargetObject.GetComponent<IHackObject>().nowHackEvent.Remove(this);
    }

    protected void Tracking()
    {
        this.transform.position = hackTargetObject.transform.position;
        this.transform.localEulerAngles = hackTargetObject.transform.localEulerAngles;
    }

    protected abstract void Enter();

    protected abstract void Execute();

    protected abstract void Exit();

    // 各メソッド起動、継承クラスにUpdateは入れない
    public void Update()
    {
        if (isEventAct)
        {
            if (!isSet)
            {
                Enter();
                isSet = true;
            }

            Execute();
        }
        else if (isSet)
        {
            Exit();
            isSet = false;
        }
    }

    //public abstract void ToolAction();
}
