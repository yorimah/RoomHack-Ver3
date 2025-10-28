using UnityEngine;

public class Granade : BombCore
{
    [SerializeField, Header("爆発までの秒数")]
    private float explosionTimer = 3;

    // 汎用タイマー
    private float timer = 0;

    private SpriteRenderer spriteRen;
    private Color color;

    private float colorAlpha;

    public float hitStop;

    private CircleCollider2D granadeCollider;
    bool isExplosion;

    public void Start()
    {
        granadeCollider = GetComponent<CircleCollider2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        color = spriteRen.color;
        granadeCollider.isTrigger = false;
        MeshInit();
        SeManager.Instance.Play("BomCount");
        isExplosion = false;
    }

    private void Update()
    {
        // 爆発範囲表示
        ExplosionRadius();

        if (timer >= explosionTimer)
        {
            //爆発
            colorAlpha = 1;
            Destroy(gameObject, 1f);
            Destroy(meshObject, 1f);
            if (isExplosion)
            {
                Bomb();
                isExplosion = false;
            }
        }
        else
        {
            isExplosion = true;
            timer += GameTimer.Instance.GetScaledDeltaTime();
            colorAlpha = Mathf.Sin(Mathf.Pow(4, timer));
        }

        // 点滅        
        color.a = colorAlpha;
        spriteRen.color = color;
    }
}
