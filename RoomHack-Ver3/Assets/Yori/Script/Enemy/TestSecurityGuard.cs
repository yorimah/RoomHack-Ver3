using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestSecurityGuard : MonoBehaviour, IHackObject, IDamegeable
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
    [SerializeField, Header("障害物に使うレイヤー")]
    private LayerMask obstacleMask;

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


    private int shotRate = 3;

    float shotIntevalTime = 0;
    private int shotNum = 0;

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

        shotIntevalTime = 1f / shotRate;
    }
    void FixedUpdate()
    {
        actFuncTbl[(int)actNo]();
        RotationFoward();
        Hacking();
    }

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
                if (aimTime <= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                break;
            case ShotSection.shot:
                shootDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
                GunFire();
                shotNum++;
                shotSection++;
                break;
            case ShotSection.shotInterval:
                timer += Time.deltaTime;
                if (shotIntevalTime <= timer)
                {
                    timer = 0;
                    if (shotNum >= shotRate)
                    {
                        shotNum = 0;
                        actNo = ActNo.move;
                        shotSection = ShotSection.aim;
                    }
                    shotSection = ShotSection.shot;
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
        bulletCore.power = 40;
        bulletCore.hitStop = 0.1f;
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

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;

        Handles.Label(transform.position + Vector3.up * 1f, "actNo " + actNo.ToString(), style);
        Handles.Label(transform.position + Vector3.up * 1.5f, "shotSection " + shotSection.ToString(), style);
        Handles.Label(transform.position + Vector3.up * 2f, "HP " + NowHP.ToString(), style);
        Handles.Label(transform.position + Vector3.up * 2.5f, "shotIntevalTime " + shotIntevalTime.ToString(), style);
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

        Vector2 center = PlayerPosition();
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
        Vector2 playerPosition = PlayerPosition();
        float playerDistance = Vector2.Distance(transform.position, playerPosition);
        Vector2 playerDirection = (playerPosition - (Vector2)transform.position).normalized;
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
        Vector3 playerPosition = PlayerPosition();
        Vector2 direction = playerPosition - this.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;
    }

    Vector2 PlayerPosition()
    {
        if (UnitCore.Instance != null)
        {
            return UnitCore.Instance.transform.position;
        }
        Debug.LogError("playerみつかんないよ～ん");
        return Vector2.zero;
    }
    public bool hacked { get; set; } = false;
    float hackSpeed = 0;
    float hackDamage = 0;
    float hackTimer = 0;
    public void HackStart(int _hackSpeed, int _hackDamage)
    {
        hacked = true;
        hackSpeed = _hackSpeed;
        hackDamage = _hackDamage;
        hackTimer = 0;
    }

    void Hacking()
    {
        if (hacked)
        {
            hackTimer += Time.deltaTime;
            if (hackSpeed <= hackTimer)
            {
                shotIntevalTime = 2f / shotRate;
            }
            if (hackDamage <= hackTimer)
            {
                shotIntevalTime = 1f / shotRate;
                hacked = false;
            }
        }

    }
}
