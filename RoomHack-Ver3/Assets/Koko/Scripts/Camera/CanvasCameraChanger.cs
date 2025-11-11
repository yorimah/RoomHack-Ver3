using UnityEngine;

public class CanvasCameraChanger : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    Camera actionCamera;

    [SerializeField, Header("要アタッチ")]
    Camera hackCamera;

    Canvas hackCanvas;

    private void Start()
    {
        hackCanvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (GameTimer.Instance.IsHackTime)
        {
            hackCanvas.worldCamera = hackCamera;

        }
        else
        {
            hackCanvas.worldCamera = actionCamera;
        }
    }

}
