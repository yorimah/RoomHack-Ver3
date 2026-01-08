using UnityEngine;

public class ToolEvent_Armorust : ToolEventBase, IToolEvent_Target, IToolEvent_Time
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.Armorust;


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
    int startArmor;
    int disArmor = 20;

    GameObject effect;

    protected override void Enter()
    {
        EventAdd(hackTargetObject);

        effect = EffectManager.Instance.ActEffect_PositionTrace(EffectManager.EffectType.Bad, this.gameObject, Vector2.zero);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startArmor = targetData.armorInt;

        timer = setTime;
    }

    protected override void Execute()
    {
        // 稼働処理
        if (startArmor - disArmor < 0)
        {
            targetData.armorInt = 0;
        }
        else
        {
            targetData.armorInt = startArmor - disArmor;
        }

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
        targetData.armorInt = startArmor;
        EventRemove(hackTargetObject);
        effect.GetComponent<ParticleSystem>().Stop();
    }
}
