using UnityEngine;

public class GranadeCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 4;

    public float hitStop;

    private Rigidbody2D rb;

    [SerializeField, Header("爆発までの秒数")]
    private float exprosionTimer = 3;
    [SerializeField, Header("爆発半径")]
    private float exprosionRadial = 3;
    // 汎用タイマー
    private float timer = 0;
    [SerializeField, Header("爆発威力")]
    private int exprosionPower;


    SpriteRenderer spriteRen;
    Color color;

    float colorAlpha;
    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        MAXHP = 1;
        NowHP = MAXHP;
        granadeCollider = GetComponent<CircleCollider2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        color = spriteRen.color;
    }

    private CircleCollider2D granadeCollider;
    private void Update()
    {
        if (timer >= exprosionTimer)
        {
            //爆発
            if (granadeCollider.radius >= exprosionRadial)
            {
                Die();
            }
            else
            {
                granadeCollider.radius += 3 * GameTimer.Instance.ScaledDeltaTime;
            }
        }
        else
        {
            timer += GameTimer.Instance.ScaledDeltaTime;
            colorAlpha = Mathf.Sin(Mathf.Pow(4, timer));
        }
        // 点滅        
        color.a = colorAlpha;
        spriteRen.color = color;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // IDamegebableが与えられるか調べる。与えられるならdmglayerを調べて当たるか判断
        if (collision.gameObject.TryGetComponent<IDamegeable>(out var damage))
        {
            if (this.HitDamegeLayer != damage.HitDamegeLayer)
            {
                // 障害物に当たったら即爆発
                if (HitDamegeLayer == 3)
                {
                    timer = exprosionTimer;
                }
                else
                {
                    damage.HitDmg(exprosionPower, hitStop);
                    Die();
                }
            }
        }
    }
}
