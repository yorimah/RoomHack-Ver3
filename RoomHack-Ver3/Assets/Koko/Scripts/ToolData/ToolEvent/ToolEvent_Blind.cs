using UnityEngine;

public class ToolEvent_Blind : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.Blind;

    float timer = 0;
    float lifeTime = 5;

    Enemy targetData;
    float startInterval;

    GameObject effect;

    protected override void Enter()
    {
        EventAdd();

        effect = EffectManager.Instance.ActEffect(EffectManager.EffectType.Bad, this.gameObject);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startInterval = targetData.shotIntervalTime;

        timer = lifeTime;
    }

    protected override void Execute()
    {

        // 稼働処理
        targetData.shotIntervalTime = 9999;
        Tracking();

        // 終了設定
        timer -= GameTimer.Instance.ScaledDeltaTime;
        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.died))
        {
            EventEnd();
        }
    }

    protected override void Exit()
    {

        targetData.shotIntervalTime = startInterval;
        EventRemove();
        effect.GetComponent<ParticleSystem>().Stop();
    }

    //private void Update()
    //{
    //    if (isEventAct)
    //    {
    //        // 初期設定
    //        if (!isSet)
    //        {
    //            HackSetup();
    //            isSet = true;
    //        }

    //        // 稼働処理
    //        targetData.shotIntervalTime = 9999;
    //        Tracking();

    //        // 終了設定
    //        timer -= GameTimer.Instance.ScaledDeltaTime;
    //        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.died))
    //        {
    //            targetData.shotIntervalTime = startInterval;
    //            EventRemove();
    //            effect.GetComponent<ParticleSystem>().Stop();
    //            isSet = false;
    //            isEventAct = false;
    //        }
    //    }
    //}

    //// 初期設定
    //void HackSetup()
    //{
    //}

    //public override void ToolAction()
    //{
    //}
}
