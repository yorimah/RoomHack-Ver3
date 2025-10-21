using UnityEngine;

public class GameTimer
{
    private static GameTimer _instance;
    public static GameTimer Instance => _instance ??= new GameTimer();

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

    public bool isHackMode = false;

    public void SetTimeScale(float scale)
    {
        customTimeScale = Mathf.Clamp(scale, 0f, 5f);
    }

    public void SetHackModeTimeScale() 
    {
        SetGlobalTimeScale(0.1f);
        isHackMode = true;
    }
    public void SetAcitionModeTimeScale()
    {
        SetGlobalTimeScale(1f);
        isHackMode = false;
    }
    public void SetGlobalTimeScale(float scale)
    {
        Time.timeScale = Mathf.Clamp(scale, 0f, 5f);
    }

    public void PauseGame() => SetGlobalTimeScale(0f);

    public void ResumeGame() => SetGlobalTimeScale(1f);
}
