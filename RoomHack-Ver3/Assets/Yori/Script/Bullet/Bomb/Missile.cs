using UnityEngine;

public class Missile : BombCore, IDamageable
{
    [SerializeField, Header("爆発までの秒数")]
    private float explosionTimer = 3;

    public float maxHitPoint { get; set; }

    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 4;

    public float hitStop { get; set; }
    // 汎用タイマー
    private float timer = 0;

    private SpriteRenderer spriteRen;
    private Color color;

    private float colorAlpha;

    private Rigidbody2D rb;

    [SerializeField, Header("ミサイル速度")]
    private float missileSpeed;

    private bool isFire;

    [SerializeField, Header("加速するまでの時間")]
    private float boostTime = 0.5f;

    private float boost;

    [SerializeField, Header("最初の減速")]
    private float deceleration;

    // エフェクト用
    GameObject effect;

    public void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        color = spriteRen.color;
        MeshInit();
        timer = 0;
        nowHitPoint = maxHitPoint;
        SeManager.Instance.Play("MissleMove");

        // missileエフェクト用
        effect = EffectManager.Instance.ActEffect(EffectManager.EffectType.MissileFire, this.gameObject);
    }

    private void Update()
    {
        // 爆発範囲表示
        ExplosionRadius();

        if (isFire)
        {
            // 爆発
            Explosion();
            Die();
        }
        else
        {
            // 追尾移動
            MissileMove();

            if (timer >= explosionTimer)
            {                
                EffectManager.Instance.ActEffect(EffectManager.EffectType.Bomb, this.transform.position, 0, true);
                isFire = true;
            }
            else
            {
                timer += GameTimer.Instance.ScaledDeltaTime;
            }

            // 点滅処理
            colorAlpha = Mathf.Sin(Mathf.Pow(4, timer));
            color.a = colorAlpha;
            spriteRen.color = color;
        }
    }

    // 追尾中
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damage))
        {
            SeManager.Instance.StopImmediately("MissileMove");
            // 撃ったやつの種類以外に当たって爆発
            if (hitDamegeLayer != damage.hitDamegeLayer)
            {
                EffectManager.Instance.ActEffect(EffectManager.EffectType.Bomb, this.transform.position, 0, true);
                SeManager.Instance.Play("Explosion");
                isFire = true;
            }
        }
    }
    [Range(0f, 1f)]
    public float inertia = 0.9f;       // 慣性の強さ（大きいほど鈍い）
    private void MissileMove()
    {

        // 最初は遅い
        if (timer <= boostTime)
        {
            boost = deceleration;
        }
        // 通常
        else
        {
            boost = 1;
        }
        // ターゲット方向
        Vector2 toTarget = ((Vector2)Player.Instance.transform.position - rb.position).normalized;

        // 現在の速度方向（正規化）
        Vector2 currentDir = rb.linearVelocity.normalized;
        // 過去のベクトルと新しいベクトルを合成（慣性あり）
        Vector2 blendedDir = (currentDir * inertia + toTarget * (1f - inertia)).normalized;

        // 速度を更新
        rb.linearVelocity = blendedDir * missileSpeed * boost * GameTimer.Instance.customTimeScale;

        // 回転を進行方向に合わせる
        float angle = Mathf.Atan2(blendedDir.y, blendedDir.x) * Mathf.Rad2Deg - 90f;
        rb.MoveRotation(angle);

    }
    public void Die()
    {
        effect.SetActive(false);
        Destroy(gameObject);
        Destroy(meshObject, 0.5f);
    }
    public void Explosion()
    {
        rb.linearVelocity = Vector2.zero;
        colorAlpha = 0;
        Bomb();
    }
}
