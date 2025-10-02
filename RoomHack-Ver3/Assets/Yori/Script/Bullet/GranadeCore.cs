using UnityEngine;

public class GranadeCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 4;

    public float hitStop;

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

    [SerializeField]
    LayerMask targetLm;
    public void Start()
    {
        MAXHP = 1;
        NowHP = MAXHP;
        granadeCollider = GetComponent<CircleCollider2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        color = spriteRen.color;
        MeshInit();
    }

    private CircleCollider2D granadeCollider;
    private void Update()
    {
        // 爆発範囲表示
        ExplosionRadius();
        if (timer >= explosionTimer)
        {
            //爆発
            if (granadeCollider.radius >= explosionRadial)
            {

            }
            else
            {
                granadeCollider.radius += 2 * GameTimer.Instance.ScaledDeltaTime;
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
        Destroy(meshObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // IDamegebableが与えられるか調べる。与えられるならdmglayerを調べて当たるか判断
        if (collision.gameObject.TryGetComponent<IDamegeable>(out var damage))
        {
            Debug.Log("ダメージを与えられる" + collision.gameObject.name + "と接触");
            if (this.HitDamegeLayer != damage.HitDamegeLayer)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position);
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject == collision.gameObject)
                {
                    Debug.Log("グレネード");
                    damage.HitDmg(explosionPower, hitStop);
                }
            }
        }
    }

    // 爆発範囲表示
    private void MeshInit()
    {
        meshObject = new GameObject(gameObject.name + " ExplosionRadiusMesh");
        meshObject.AddComponent<MeshFilter>();
        var mr = meshObject.AddComponent<MeshRenderer>();
        // メッシュの色設定、ここでいじれる
        mr.material = new Material(Shader.Find("Unlit/Color"));
        mr.material.color = new Color(0, 1, 0, 0.1f); // 半透明緑
        mr.sortingOrder = -1;
        mesh = new Mesh();
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
    }

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
}
