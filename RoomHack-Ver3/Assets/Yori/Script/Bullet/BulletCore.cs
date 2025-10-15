using UnityEngine;

public class BulletCore : MonoBehaviour, IDamageable
{
    public float maxHitPoint { get; set; }

    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; }

    public int power;

    public float hitStop;

    private Rigidbody2D rb;

    private Vector2 initVel;
    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        maxHitPoint = 1;
        nowHitPoint = maxHitPoint;

        initVel = rb.linearVelocity;

        //SeManager.Instance.Play("9mm");
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        // タイムスケールが1より大きいならそのままの速度、小さいならGameTimer.Instance.customTimeScale分遅くなる
        rb.linearVelocity = 1 < GameTimer.Instance.customTimeScale ? initVel : initVel * GameTimer.Instance.customTimeScale;
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
                if (damage.hitDamegeLayer != 3) EffectManager.Instance.ActEffect(EffectManager.EffectType.HitDamage, this.transform.position, -(this.transform.localEulerAngles.z) - 90, true);

                damage.HitDmg(power, hitStop);
                Die();
            }
        }
    }
}
