using UnityEngine;

public class ToolEvent_Bind : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.Bind;

    float timer = 0;
    float lifeTime = 5;

    Enemy targetData;
    float startSpeed;

    GameObject effect;

    protected override void Enter()
    {
        // 開始処理
        EventAdd();

        effect = EffectManager.Instance.ActEffect(EffectManager.EffectType.Bad, this.gameObject);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startSpeed = targetData.moveSpeed;

        timer = lifeTime;
    }

    protected override void Execute()
    {
        // 稼働処理
        targetData.moveSpeed = 0;
        Tracking();


        // 終了条件
        timer -= GameTimer.Instance.ScaledDeltaTime;
        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.died))
        {
            EventEnd();
        }
    }

    protected override void Exit()
    {
        // 終了処理
        targetData.moveSpeed = startSpeed;
        EventRemove();
        effect.GetComponent<ParticleSystem>().Stop();
    }


    //public override void ToolAction()
    //{
    //}
}
