using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_OverHeat : ToolEventBase, IToolEventBase_Target
{
    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }


    public override toolTag thisToolTag { get; set; } = toolTag.OverHeat;

    float timer = 0;
    float lifeTime = 5;

    float damageTimer = 0;
    float damageTime = 1;
    int damage = 10;

    IDamageable damageable;

    GameObject effect;

    protected override void Enter()
    {
        EventAdd(hackTargetObject);

        effect = EffectManager.Instance.ActEffect(EffectManager.EffectType.Fire, this.gameObject, 0);

        damageable = hackTargetObject.GetComponent<IDamageable>();

        timer = lifeTime;
        damageTimer = damageTime;
    }

    protected override void Execute()
    {
        // 稼働処理
        damageTimer -= GameTimer.Instance.GetScaledDeltaTime();
        if (damageTimer <= 0)
        {
            damageable.HitDmg(damage, 0);
            damageTimer = damageTime;
        }
        Tracking(hackTargetObject);

        // 終了設定
        timer -= GameTimer.Instance.GetScaledDeltaTime();
        if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.died))
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
