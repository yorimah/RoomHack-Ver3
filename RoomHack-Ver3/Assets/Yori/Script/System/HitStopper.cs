using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopper : MonoBehaviour
{
    public static HitStopper Instance { get; private set; }
    [SerializeField,Header("cinemachine")]
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
        cinemachineImpulse.GenerateImpulse();
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration); 
        Time.timeScale = originalTimeScale;
    }
}
