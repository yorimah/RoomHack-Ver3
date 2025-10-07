using UnityEngine;

public class BulletCore : MonoBehaviour, IDamageable
{
    public float MAXHP { get; set; }

    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; }

    public int power;

    public float hitStop;

    private Rigidbody2D rb;

    private Vector2 initVel;
    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        MAXHP = 1;
        NowHP = MAXHP;

        initVel = rb.velocity;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        // タイムスケールが1より大きいならそのままの速度、小さいならGameTimer.Instance.customTimeScale分遅くなる
        rb.velocity = 1 < GameTimer.Instance.customTimeScale ? initVel : initVel * GameTimer.Instance.customTimeScale;
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
            if (this.HitDamegeLayer != damage.HitDamegeLayer)
            {
                damage.HitDmg(power, hitStop);
                Die();
            }
        }
    }
}
