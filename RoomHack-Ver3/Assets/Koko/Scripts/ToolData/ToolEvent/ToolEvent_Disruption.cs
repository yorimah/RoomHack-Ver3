using UnityEngine;

public class ToolEvent_Disruption : ToolEventBase, IToolEvent_Attack, IToolEvent_Target, IToolEvent_ToolManager
{

    public override ToolTag thisToolTag { get; set; } = ToolTag.Disruption;

    // IToolEvent_Attack
    public IDamageable damageable { get; set; }
    public int damage { get; set; } = 0;
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


    // ToolManager
    public ToolManager toolManager { get; set; }
    public void GetToolManager()
    {
        toolManager = ToolManager.Instance;
    }


    protected override void Enter()
    {

        GetDamageable(hackTargetObject);
        GetToolManager();

        EffectManager.Instance.ActEffect(EffectManager.EffectType.HitDie, hackTargetObject.transform.position);

    }

    protected override void Execute()
    {
        int rand = Random.Range(0, toolManager.GetHandData().Count - 1);
        int cost = toolManager.toolDataBank.toolDataList[(int)toolManager.GetHandData()[rand]].toolCost;
        toolManager.HandTrash(rand);

        damage = cost * 10;
        damageable.HitDmg(damage, 0);
        //EffectManager.Instance.ActEffect_Num(damage, hackTargetObject.transform.position, 1);

        EventEnd();
    }

    protected override void Exit()
    {
    }
}
