using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [Header("時間スケール設定")]
    /// <summary>
    /// タイムスケールの大本、ここを1/10にすると1/10の時間の進みになる。0～5の間
    /// </summary>
    [Range(0f, 5f)] public float customTimeScale = 1f;

    /// <summary>
    /// カスタムタイムスケールに応じて変化する時間
    /// </summary>
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
