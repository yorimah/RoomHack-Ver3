using UnityEngine;

public class BulletCore : MonoBehaviour, IDamageable
{
    public float MaxHitPoint { get; set; }

    public float NowHitPoint { get; set; }

    public int hitDamegeLayer { get; set; }

    public int stoppingPower;

    public float hitStopTime;

    private Rigidbody2D rb;

    private Vector2 initVel;
    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        MaxHitPoint = 1;
        NowHitPoint = MaxHitPoint;

        initVel = rb.linearVelocity;

        SeManager.Instance.Play("9mm");
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    
    private void Update()
    {
        // タイムスケールが1より大きいならそのままの速度、小さいならGameTimer.Instance.customTimeScale分遅くなる
        rb.linearVelocity = GameTimer.Instance.IsHackTime ? initVel * GameTimer.Instance.GetCustomTimeScale() : initVel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<BulletCore>(out var bullet))
        {
            return;
        }
        // IDamegebableが与えられるか調べる。与えられるならdmglayerを調べて当たるか判断
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damage))
        {
            if (this.hitDamegeLayer != damage.hitDamegeLayer)
            {
                // 壁以外ならヒットエフェクト
                if (damage.hitDamegeLayer != 3)
                {
                    EffectManager.Instance.ActEffect(EffectManager.EffectType.HitDamage, this.transform.position, (this.transform.localEulerAngles.z + 90), true);
                    EffectManager.Instance.ActEffect_Num(stoppingPower, this.transform.position, 1f);
                }
                else
                {
                    EffectManager.Instance.ActEffect(EffectManager.EffectType.HitWall, this.transform.position, (this.transform.localEulerAngles.z + 90), true);
                }

                damage.HitDmg(stoppingPower, hitStopTime);
                Die();
            }
        }
    }

    public void HitDmg(int dmg, float hitStop)
    {
        NowHitPoint -= dmg;
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
}
