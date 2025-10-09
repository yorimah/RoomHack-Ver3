using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEvent_OverHeat : ToolEvent
{
    float timer = 5;

    float damageTimer = 1;
    int damage = 10;

    IDamageable damageable;

    private void Start()
    {
        EventAdd();

        damageable = hackTargetObject.GetComponent<IDamageable>();
    }

    private void Update()
    {
        Tracking();

        damageTimer -= GameTimer.Instance.ScaledDeltaTime;
        if (damageTimer <= 0)
        {
            damageable.HitDmg(damage, 0);
            damageTimer = 1;
        }


        timer -= GameTimer.Instance.ScaledDeltaTime;

        if (timer < 0)
        {
            EventRemove();
        }
    }

    public override void ToolAction()
    {
    }
}
