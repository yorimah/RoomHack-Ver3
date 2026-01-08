using UnityEngine;

public class ToolEvent_BitClack : ToolEventBase, IToolEvent_Attack, IToolEvent_Target
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.BitClack;


    // IToolEvent_Attack
    public IDamageable damageable { get; set; }
    public int damage { get; set; } = 10;
    public void GetDamageable(GameObject _hackTargetObject)
    {
        damageable = _hackTargetObject.GetComponent<IDamageable>();
    }

    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }


    protected override void Enter()
    {
        GetDamageable(hackTargetObject);

        EffectManager.Instance.ActEffect(EffectManager.EffectType.HitDie, hackTargetObject.transform.position);

        //EventAdd();
    }

    protected override void Execute()
    {
        damageable.HitDmg(damage, 0);
        //EffectManager.Instance.ActEffect_Num(damage, hackTargetObject.transform.position, 1);

        EventEnd();
    }

    protected override void Exit()
    {
        //EventRemove();
    }



}
