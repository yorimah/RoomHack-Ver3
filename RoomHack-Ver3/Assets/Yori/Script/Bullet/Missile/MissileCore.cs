using UnityEngine;

public class MissileCore : MonoBehaviour
{
    [SerializeField, Header("爆発までの秒数")]
    private float explosionTimer = 3;
    [SerializeField, Header("爆発半径")]
    private float explosionRadial = 3;
    [SerializeField, Header("爆発威力")]
    private int explosionPower;

    // 汎用タイマー
    private float timer = 0;

    private SpriteRenderer spriteRen;
    private Color color;

    private float colorAlpha;

    // 分割数
    private int segment = 360;
    private Mesh mesh;
    private GameObject meshObject;

    [SerializeField, Header("壁")]
    private LayerMask targetLm;

    private int HitDamegeLayer = 2;

    public float hitStop;

    private CircleCollider2D missileCol;

    private Rigidbody2D rb;

    [SerializeField, Header("ミサイル速度")]
    private float missileSpeed;

    private bool isFire;

    public void Start()
    {
        missileCol = GetComponent<CircleCollider2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        color = spriteRen.color;
        missileCol.isTrigger = false;
        MeshInit();
        timer = 0;
    }

    private void Update()
    {
        // 爆発範囲表示
        ExplosionRadius();

        if (isFire)
        {
            // 爆発
            rb.velocity = Vector2.zero;
            colorAlpha = 0;
            Destroy(gameObject, 0.5f);
            Destroy(meshObject, 0.5f);
            missileCol.isTrigger = true;
            missileCol.radius = explosionRadial;
        }
        else
        {
            // 追尾移動
            HomingMissile();

            if (timer >= explosionTimer)
            {
                isFire = true;
            }
            else
            {
                timer += GameTimer.Instance.ScaledDeltaTime;
            }

            // プレイヤーの方に向く
            RotationForward();

            // 点滅処理
            colorAlpha = Mathf.Sin(Mathf.Pow(4, timer));
            color.a = colorAlpha;
            spriteRen.color = color;
        }
    }
    [SerializeField, Header("加速するまでの時間")]
    private float boostTime = 0.5f;

    private float boost;

    [SerializeField, Header("最初の減速")]
    private float deceleration;
    private void HomingMissile()
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


        rb.velocity = Vector3.up * missileSpeed * boost * GameTimer.Instance.customTimeScale;
    }

    //  爆風
    private void OnTriggerStay2D(Collider2D collision)
    {
        // IDamegebableが与えられるか調べる。与えられるならdmglayerを調べて当たるか判断
        if (collision.gameObject.TryGetComponent<IDamegeable>(out var damage))
        {
            if (HitDamegeLayer != damage.HitDamegeLayer)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position - transform.position);
                if (hit.collider.gameObject == collision.gameObject)
                {
                    damage.HitDmg(explosionPower, hitStop);
                }
            }
        }
    }

    // 追尾中
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamegeable>(out var damage))
        {
            // 撃ったやつの種類以外に当たって爆発
            if (HitDamegeLayer != damage.HitDamegeLayer)
            {
                isFire = true;
            }
        }
    }

    // mesh初期化
    private void MeshInit()
    {
        meshObject = new GameObject(gameObject.name + " ExplosionRadiusMesh");
        meshObject.AddComponent<MeshFilter>();
        var mr = meshObject.AddComponent<MeshRenderer>();
        // メッシュの色設定、ここでいじれる
        mr.material = new Material(Shader.Find("Custom/URP_SpriteSimple"));
        mr.material.color = new Color(0.75f, 0, 0, 0.5f);
        mr.sortingOrder = -1;
        mesh = new Mesh();
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    // 爆発範囲表示
    private void ExplosionRadius()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心は自分
        vertices[0] = transform.position;

        for (int i = 0; i < segment; i++)
        {
            float angle = (360f / segment) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, explosionRadial, targetLm);

            if (hit.collider != null)
            {
                // 障害物に当たったらその地点を頂点にする
                vertices[i + 1] = hit.point;
            }
            else
            {
                // 何もなければ円周上の点
                vertices[i + 1] = transform.position + dir * explosionRadial;
            }

            // 三角形の設定
            if (i < segment - 1)
            {
                int start = i * 3;
                triangles[start] = 0;
                triangles[start + 1] = i + 2;
                triangles[start + 2] = i + 1;
            }
            else
            {
                // 最後は1番目の点とつなげる
                int start = i * 3;
                triangles[start] = 0;
                triangles[start + 1] = 1;
                triangles[start + 2] = i + 1;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void RotationForward()
    {
        Vector2 playerPosition = UnitCore.Instance.transform.position;
        Vector2 playerNextPosition = playerPosition + UnitCore.Instance.GetComponent<Rigidbody2D>().velocity.normalized;
        Vector2 direction = playerNextPosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;
    }
}
