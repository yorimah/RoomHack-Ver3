using UnityEngine;

public interface IToolEvent_Target
{
    public GameObject hackTargetObject { get; set; }

    public void Tracking(GameObject _object);
}
