using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_OverHeat : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.OverHeat;

    bool isSet = false;
    float timer = 0;
    float lifeTime = 5;

    float damageTimer = 0;
    float damageTime = 1;
    int damage = 10;

    IDamageable damageable;

    GameObject effect;

    private void Update()
    {
        if (isEventAct)
        {
            // 初期設定
            if (!isSet)
            {
                HackSetup();
                isSet = true;
            }

            // 稼働処理
            damageTimer -= GameTimer.Instance.ScaledDeltaTime;
            if (damageTimer <= 0)
            {
                damageable.HitDmg(damage, 0);
                damageTimer = damageTime;
            }
            Tracking();

            // 終了設定
            timer -= GameTimer.Instance.ScaledDeltaTime;
            if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.died))
            {
                EventRemove();
                effect.GetComponent<ParticleSystem>().Stop();
                isSet = false;
                isEventAct = false;
            }
        }
    }

    void HackSetup()
    {
        EventAdd();

        effect = EffectManager.Instance.ActEffect(EffectManager.EffectType.Fire, this.gameObject);

        damageable = hackTargetObject.GetComponent<IDamageable>();

        timer = lifeTime;
        damageTimer = damageTime;
    }

    public override void ToolAction()
    {
    }
}
