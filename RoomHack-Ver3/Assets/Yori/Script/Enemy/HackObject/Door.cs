using System.Collections.Generic;
using UnityEngine;
public class Door : MonoBehaviour, IDamageable, IHackObject
{
    public int armorInt { get; set; }

    [SerializeField, Header("装甲")]
    private int armorSerialze = 0;

    public string HackObjectName { get; protected set; }
    public List<ToolType> cantHackToolType { get; set; } = new List<ToolType>();
    public List<ToolEventBase> nowHackEvent { get; set; } = new List<ToolEventBase>();

    public bool CanHack { get; set; } = false;

    public bool IsView { get; set; }

    // ダメージ関連
    public float MaxHitPoint { get; private set; } = 5;
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 2;

    [SerializeField, Header("ハックダメージ倍率")]
    private int hackMag = 10;

    [SerializeField, Header("HP")]
    private int serializeHitPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private BoxCollider2D boxCollider;
    void Start()
    {
        MaxHitPoint = serializeHitPoint;
        NowHitPoint = MaxHitPoint;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HitDmg(int dmg, float hitStop)
    {
        // 防護点上回ってHP回復したわバカタレ
        int damage = dmg - armorInt;
        if (damage < 0) damage = 0;
        NowHitPoint -= damage;
        EffectManager.Instance.ActEffect_Num(damage, this.transform.position, 1);

        if (NowHitPoint <= 0)
        {
            Die();
        }
        else
        {
            if (hitDamegeLayer == 2)
            {
                SeManager.Instance.Play("Hit");
            }
            HitStopper.Instance.StopTime(hitStop);
        }
    }

    public void HackDmg(int dmg, float hitStop)
    {
        // ハックダメージに防護点計算してたら意味ないやんけ！おい！
        NowHitPoint -= dmg * hackMag;
        EffectManager.Instance.ActEffect_Num(dmg, this.transform.position, 1);

        if (NowHitPoint <= 0)
        {
            Die();
        }
        else
        {
            if (hitDamegeLayer == 2)
            {
                SeManager.Instance.Play("Hit");
            }
            HitStopper.Instance.StopTime(hitStop);
        }
    }
    public void Die()
    {
        boxCollider.size = Vector2.zero;
    }
}
