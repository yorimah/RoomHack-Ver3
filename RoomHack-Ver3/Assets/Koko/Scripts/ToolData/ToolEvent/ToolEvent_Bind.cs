using UnityEngine;

public class ToolEvent_Bind : ToolEventBase, IToolEventBase_Target
{
    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }


    public override toolTag thisToolTag { get; set; } = toolTag.Bind;

    float timer = 0;
    float lifeTime = 5;

    Enemy targetData;
    float startSpeed;

    GameObject effect;

    protected override void Enter()
    {
        // 開始処理
        EventAdd(hackTargetObject);

        effect = EffectManager.Instance.ActEffect(EffectManager.EffectType.Bad, this.gameObject);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startSpeed = targetData.moveSpeed;

        timer = lifeTime;
    }

    protected override void Execute()
    {
        // 稼働処理
        targetData.moveSpeed = 0;
        Tracking(hackTargetObject);


        // 終了条件
        timer -= GameTimer.Instance.GetScaledDeltaTime();
        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.died))
        {
            EventEnd();
        }
    }

    protected override void Exit()
    {
        // 終了処理
        targetData.moveSpeed = startSpeed;
        EventRemove(hackTargetObject);
        effect.GetComponent<ParticleSystem>().Stop();
    }
}
