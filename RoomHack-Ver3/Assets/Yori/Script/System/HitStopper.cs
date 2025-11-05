using Cinemachine;
using System.Collections;
using UnityEngine;

public class HitStopper : MonoBehaviour
{
    public static HitStopper Instance { get; private set; }
    [SerializeField, Header("cinemachine")]
    private CinemachineVirtualCamera mainCamera;
    private CinemachineImpulseSource cinemachineImpulse;
    private void Awake()
    {
        Instance = this;
        cinemachineImpulse = mainCamera.GetComponent<CinemachineImpulseSource>();
    }

    public void StopTime(float duration)
    {
        StartCoroutine(HitStopCoroutine(duration));
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        GameTimer.Instance.SetCustumTimeScale(0);
        yield return new WaitForSecondsRealtime(duration);
        GameTimer.Instance.SetCustumTimeScale(GameTimer.Instance.GetCustomTimeScale());
    }
}
