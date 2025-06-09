using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopper : MonoBehaviour
{
    public static HitStopper Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void StopTime(float duration)
    {
        Debug.Log("ヒットストップ!");
        StartCoroutine(HitStopCoroutine(duration));
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration); 
        Time.timeScale = originalTimeScale;
    }
}
