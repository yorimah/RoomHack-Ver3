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
        damageable = targetObject.GetComponent<IDamageable>();
    }

    private void Update()
    {
        damageTimer -= GameTimer.Instance.ScaledDeltaTime;
        if (damageTimer <= 0)
        {
            damageable.HitDmg(damage, 0);
            damageTimer = 1;
        }


        timer -= GameTimer.Instance.ScaledDeltaTime;

        if (timer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override void ToolAction()
    {
    }
}
