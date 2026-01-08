using UnityEngine;

public class ToolEvent_DeadRock : ToolEventBase, IToolEvent_Target, IToolEvent_Time
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.DeadRock;


    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }

    // IToolEvent_Time
    public float setTime { get; set; } = 3;
    public float timer { get; set; } = 0;


    Enemy targetData;
    float startSpeed;

    GameObject effect;

    protected override void Enter()
    {
        // 開始処理
        EventAdd(hackTargetObject);

        effect = EffectManager.Instance.ActEffect_PositionTrace(EffectManager.EffectType.Bad, this.gameObject, Vector2.zero);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startSpeed = targetData.moveSpeed;

        timer = setTime;
    }

    protected override void Execute()
    {
        // 稼働処理
        targetData.moveSpeed = 0;
        Tracking(hackTargetObject);


        // 終了条件
        timer -= GameTimer.Instance.GetScaledDeltaTime();
        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.isDead))
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
