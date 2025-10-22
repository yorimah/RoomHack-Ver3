using UnityEngine;

public interface IToolEventBase_Target
{
    public GameObject hackTargetObject { get; set; }

    public void Tracking(GameObject _object);
}
