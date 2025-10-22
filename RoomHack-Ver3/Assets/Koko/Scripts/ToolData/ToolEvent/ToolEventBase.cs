using UnityEngine;

public abstract class ToolEventBase : MonoBehaviour
{
    [HideInInspector]
    public abstract toolTag thisToolTag { get; set; }

    //public GameObject hackTargetObject;

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

    protected void EventAdd(GameObject _gameObject)
    {
        _gameObject.GetComponent<IHackObject>().nowHackEvent.Add(this);
    }

    protected void EventRemove(GameObject _gameObject)
    {
        _gameObject.GetComponent<IHackObject>().nowHackEvent.Remove(this);
    }

    //protected void Tracking(GameObject _gameObject)
    //{
    //    this.transform.position = _gameObject.transform.position;
    //    this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    //}

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
