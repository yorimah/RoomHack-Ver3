using System.Collections;
using UnityEngine;

public class ToolEvent_OverHeat : ToolEventBase, IToolEvent_Target, IToolEvent_Time, IToolEvent_Attack
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

    // IToolEvent_Attack
    public IDamageable damageable { get; set; }
    public int damage { get; set; } = 10;
    public void GetDamageable(GameObject _hackTargetObject)
    {
        damageable = _hackTargetObject.GetComponent<IDamageable>();
    }



    public override ToolTag thisToolTag { get; set; } = ToolTag.OverHeat;

    float damageTimer = 0;
    float damageTime = 1;


    GameObject effect;

    protected override void Enter()
    {
        EventAdd(hackTargetObject);

        effect = EffectManager.Instance.ActEffect_PositionTrace(EffectManager.EffectType.Fire, this.gameObject, Vector2.zero);

        GetDamageable(hackTargetObject);

        timer = setTime;
        damageTimer = damageTime;
    }

    protected override void Execute()
    {
        // 稼働処理
        damageTimer -= GameTimer.Instance.GetScaledDeltaTime();
        if (damageTimer <= 0)
        {
            damageable.HackDmg(damage, 0);
            damageTimer = damageTime;
        }
        Tracking(hackTargetObject);

        // 終了設定
        timer -= GameTimer.Instance.GetScaledDeltaTime();
        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.isDead))
        {
            
            effect.GetComponent<ParticleSystem>().Stop();
            EventEnd();
        }
    }

    protected override void Exit()
    {
        EventRemove(hackTargetObject);
    }
}
