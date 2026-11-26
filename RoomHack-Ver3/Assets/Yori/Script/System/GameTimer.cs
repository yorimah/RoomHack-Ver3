using UnityEngine;
public class GameTimer
{
    private static readonly GameTimer _instance = new GameTimer();

    public static GameTimer Instance => _instance;

    /// <summary>
    /// タイムスケールの大本、ここを1/10にすると1/10の時間の進みになる。0～5の間
    /// </summary>
    [Range(0f, 5f)] private float customTimeScale = 1f;

    public float GetCustomTimeScale()
    {
        return customTimeScale * playTime;
    }

    /// <summary>
    /// カスタムタイムスケールに応じて変化する時間
    /// </summary>
    private float ScaledDeltaTime => Time.deltaTime * customTimeScale;

    public float GetScaledDeltaTime()
    {
        return ScaledDeltaTime * playTime;
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
    public void SetCustomTimeScale(float scale)
    {
        customTimeScale = Mathf.Clamp(scale, 0f, 5f);
    }

    // ゲーム演出用の仮変数
    // 後で要修正 bykoko
    // 0or1のみ
    public int playTime = 1;

    public void SetHackModeTimeScale()
    {
        SetCustomTimeScale(0.05f);
        IsHackTime = true;
    }
    public void SetActionModeTimeScale()
    {
        SetCustomTimeScale(1f);
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
