using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [Header("時間スケール設定")]
    [Range(0f, 5f)] public float customTimeScale = 1f;

    public float ScaledDeltaTime => Time.deltaTime * customTimeScale;
    public float ScaledUnscaledDeltaTime => Time.unscaledDeltaTime * customTimeScale;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetTimeScale(float scale)
    {
        customTimeScale = Mathf.Clamp(scale, 0f, 5f);
    }

    public void SetGlobalTimeScale(float scale)
    {
        Time.timeScale = Mathf.Clamp(scale, 0f, 5f);
    }

    public void PauseGame() => SetGlobalTimeScale(0f);
    public void ResumeGame() => SetGlobalTimeScale(1f);
}
