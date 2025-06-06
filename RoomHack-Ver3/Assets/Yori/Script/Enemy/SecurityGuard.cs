using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SecurityGuard : MonoBehaviour, IHackObject, IDamegeable
{
    public int secLevele { get; set; }

    public float MAXHP { get; set; } = 5;
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 2;

    [SerializeField, Header("弾のプレハブ")]
    private GameObject bulletPrefab;
    [SerializeField, Header("弾のスピード")]
    private float bulletSpeed;

    bool isInside;

    public float rotationSpeed = 1f;      // 回転速度（ラジアン/秒）
    public float flipInterval = 1f;       // 自動反転の周期
    public LayerMask obstacleMask;        // 障害物に使うレイヤー

    private float angle = 0f;             // 現在の角度
    private float direction = 1;
    private float flipTimer = 0f;         // 自動反転用タイマー

    // デリゲード
    // 関数を型にするためのもの
    private delegate void ActFunc();
    // 関数の配列
    private ActFunc[] actFuncTbl;

    ShotSection shotSection;
    Vector3 shootDirection;
    float aimTime = 0.5f;
    float timer = 0;
    float reloadTime = 2;


    private Rigidbody2D secRididBody;
    // リロードをここに追加
    enum ActNo
    {
        wait,
        move,
        shot,
        reload,
        num
    }
    ActNo actNo;

    // ショット関連
    int MAXMAGAGINE = 12;
    int nowMagazine = 0;

    float shotIntevalTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        actNo = ActNo.wait;
        actFuncTbl = new ActFunc[(int)ActNo.num];
        actFuncTbl[(int)ActNo.wait] = Wait;
        actFuncTbl[(int)ActNo.move] = Move;
        actFuncTbl[(int)ActNo.shot] = Shot;
        actFuncTbl[(int)ActNo.reload] = Reload;

        NowHP = MAXHP;

        nowMagazine = MAXMAGAGINE;

        timer = 0;

        secRididBody = GetComponent<Rigidbody2D>();
    }

    // プレイヤーを中心として動くときの距離
    public float radius = 3f;
    void Wait()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // 画面内判定　入ったらtrue
        isInside =
            viewportPos.x >= 0 && viewportPos.x <= 1 &&
            viewportPos.y >= 0 && viewportPos.y <= 1;
        if (isInside)
        {
            actNo = ActNo.move;
        }
    }

    void Move()
    {
        CalcPosition();
        if (WallHitCheack())
        {
            actNo = ActNo.shot;
            shotSection = ShotSection.aim;
        }
    }

    void Reload()
    {
        nowMagazine = MAXMAGAGINE;
        timer += Time.deltaTime;
        if (timer >= reloadTime)
        {
            // ショットに移動
            actNo = ActNo.shot;
            timer = 0;
        }
    }
    enum ShotSection
    {
        aim,
        shot,
        // 名前変更希望
        shotInterval,
        sum
    }


    void Shot()
    {
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:
                secRididBody.velocity = Vector2.zero;
                timer += Time.deltaTime;
                if (aimTime >= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                break;
            case ShotSection.shot:
                shootDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
                GunFire();
                shotSection++;
                break;

            case ShotSection.shotInterval:
                timer += Time.deltaTime;
                if (!WallHitCheack())
                {
                    timer = 0;
                    actNo = ActNo.move;
                    shotSection = ShotSection.aim;
                }
                if (shotIntevalTime <= timer)
                {
                    shotSection = ShotSection.shot;
                    timer = 0;
                }
                break;
            default:
                break;
        }
    }

    void GunFire()
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        bulletCore.HitDamegeLayer = this.HitDamegeLayer;
        bulletRigit.velocity = shootDirection * bulletSpeed;
        bulletGameObject.transform.up = shootDirection;

        nowMagazine--;

        // 弾が0未満だったらリロードに遷移
        if (nowMagazine <= 0)
        {
            actNo = ActNo.reload;
            shotSection = ShotSection.aim;
        }
    }
    void Update()
    {
        actFuncTbl[(int)actNo]();
        RotationFoward();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;

        Handles.Label(transform.position + Vector3.up * 1f, "actNo " + actNo.ToString(), style);
        Handles.Label(transform.position + Vector3.up * 1.5f, "shotSection " + shotSection.ToString(), style);
    }
#endif

    public void Die()
    {
        Destroy(gameObject);
    }

    Vector2 emDir;
    Vector2 nextPos;
    /// <Summary>
    /// オブジェクトの位置を計算するメソッドです。
    /// </Summary>
    void CalcPosition()
    {
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            direction = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
        }

        // プレイヤーとの距離（動的な半径）
        Vector2 center = UnitCore.Instance.transform.position;
        Vector2 dir = (Vector2)transform.position - center;

        emDir = new Vector2(-dir.y, dir.x);
        nextPos = (Vector2)transform.position + (emDir * direction);

        // 障害物チェック
        Vector2 directionToNext = (nextPos - (Vector2)transform.position).normalized;
        float checkDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToNext, checkDistance, obstacleMask);
        Debug.DrawRay(transform.position, directionToNext * checkDistance, Color.blue);
        if (hit.collider != null)
        {
            direction *= -1;
            flipTimer = 0f;
            secRididBody.velocity = Vector2.zero;
            return;
        }

        // Rigidbody2D で移動
        secRididBody.velocity = directionToNext.normalized * 5;
    }

    /// <Summary>
    /// レイを飛ばして壁にあったたらfalseあたらなかったらtrue
    /// </Summary>
    bool WallHitCheack()
    {
        float playerDistance = Vector2.Distance(transform.position, UnitCore.Instance.transform.position);
        Vector2 playerDirection = (UnitCore.Instance.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, playerDistance, obstacleMask);

        Debug.DrawRay(transform.position, playerDirection * playerDistance, Color.red);

        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }
    private void RotationFoward()
    {
        Vector3 playerPosition = UnitCore.Instance.transform.position;
        Vector2 direction = playerPosition - this.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;
    }
}
