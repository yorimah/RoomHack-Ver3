using UnityEngine;

public class GameTimer
{
    private static GameTimer _instance;

    public static GameTimer Instance => _instance ??= new GameTimer();

    /// <summary>
    /// タイムスケールの大本、ここを1/10にすると1/10の時間の進みになる。0～5の間
    /// </summary>
    [Range(0f, 5f)] private float customTimeScale = 1f;

    public float GetCustomTimeScale()
    {
        return customTimeScale;
    }

    /// <summary>
    /// カスタムタイムスケールに応じて変化する時間
    /// </summary>
    private float ScaledDeltaTime => Time.deltaTime * customTimeScale;

    public float GetScaledDeltaTime()
    {
        return ScaledDeltaTime;
    }

    private float UnscaledDeltaTime => Time.unscaledDeltaTime * customTimeScale;

    public float GetUnScaledDeltaTime()
    {
        return UnscaledDeltaTime;
    }

    public bool IsHackTime { get; private set; } = false;

    /// <summary>
    /// カスタムタイムスケールを使用してるところが遅くなります。0～5の間
    /// </summary>
    /// <param name="scale"></param>
    public void SetCustumTimeScale(float scale)
    {
        customTimeScale = Mathf.Clamp(scale, 0f, 5f);
    }

    public void SetHackModeTimeScale()
    {
        SetCustumTimeScale(0.05f);
        IsHackTime = true;
    }
    public void SetAcitionModeTimeScale()
    {
        SetCustumTimeScale(1f);
        IsHackTime = false;
    }

    /// <summary>
    /// 全体の時間をいじります。0～5の間
    /// </summary>
    /// <param name="scale"></param>
    public void SetGlobalTimeScale(float scale)
    {
        Time.timeScale = Mathf.Clamp(scale, 0f, 5f);
    }

    public void PauseGame() => SetGlobalTimeScale(0f);

    public void ResumeGame() => SetGlobalTimeScale(1f);
}
