using UnityEngine;

public class CameraEffectController : MonoBehaviour
{
    [SerializeField]
    GameObject actionCamera;

    [SerializeField]
    GameObject hackCamera;

    private void Update()
    {
        if (GameTimer.Instance.IsHackTime)
        {
            actionCamera.SetActive(false);
            hackCamera.SetActive(true);
        }
        else
        {
            actionCamera.SetActive(true);
            hackCamera.SetActive(false);
        }
    }
}
