
using System.Collections;
using UnityEngine;

public class HitStopper : MonoBehaviour
{
    public static HitStopper Instance { get; private set; }
    [SerializeField, Header("cinemachine")]
    private Unity.Cinemachine.CinemachineVirtualCamera mainCamera;
    private Unity.Cinemachine.CinemachineImpulseSource cinemachineImpulse;
    private void Awake()
    {
        Instance = this;
        cinemachineImpulse = mainCamera.GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
    }

    public void StopTime(float duration)
    {
        StartCoroutine(HitStopCoroutine(duration));
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        GameTimer.Instance.SetCustumTimeScale(0);
        yield return new WaitForSecondsRealtime(duration);
        if (GameTimer.Instance.IsHackTime)
        {
            GameTimer.Instance.SetHackModeTimeScale();
        }
        else
        {
            GameTimer.Instance.SetAcitionModeTimeScale();
        }
    }
}
