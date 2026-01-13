using UnityEngine;

public class ToolEvent_WeponGlitch : ToolEventBase, IToolEvent_Target, IToolEvent_Time
{
    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }

    // IToolEvent_Time
    public float setTime { get; set; } = 5;
    public float timer { get; set; } = 0;


    public override ToolTag thisToolTag { get; set; } = ToolTag.WeponGlitch;

    Enemy targetData;
    float startInterval;

    GameObject effect;

    protected override void Enter()
    {
        EventAdd(hackTargetObject);

        effect = EffectManager.Instance.ActEffect_PositionTrace(EffectManager.EffectType.Bad, this.gameObject, Vector2.zero);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startInterval = targetData.shotIntervalTime;

        timer = setTime;
    }

    protected override void Execute()
    {

        // 稼働処理
        targetData.shotIntervalTime = 9999;
        Tracking(hackTargetObject);

        // 終了設定
        timer -= GameTimer.Instance.GetScaledDeltaTime();
        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.isDead))
        {
            EventEnd();
        }
    }

    protected override void Exit()
    {
        targetData.shotIntervalTime = startInterval;
        EventRemove(hackTargetObject);
        effect.GetComponent<ParticleSystem>().Stop();
    }

}
