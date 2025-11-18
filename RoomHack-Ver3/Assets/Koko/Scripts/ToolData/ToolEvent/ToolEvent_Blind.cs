using UnityEngine;

public class ToolEvent_Blind : ToolEventBase, IToolEventBase_Target
{
    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }


    public override ToolTag thisToolTag { get; set; } = ToolTag.Blind;

    float timer = 0;
    float lifeTime = 5;

    Enemy targetData;
    float startInterval;

    GameObject effect;

    protected override void Enter()
    {
        EventAdd(hackTargetObject);

        effect = EffectManager.Instance.ActEffect_Trace(EffectManager.EffectType.Bad, this.gameObject, 0);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startInterval = targetData.shotIntervalTime;

        timer = lifeTime;
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
