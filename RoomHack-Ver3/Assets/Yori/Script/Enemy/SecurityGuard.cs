using System.Collections;
using UnityEngine;

public class SecurityGuard : MonoBehaviour, HackObject, IDamegeable
{
    public int secLevele { get; set; }

    public float MAXHP { get; set; } = 100;
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
    private int direction = 1;            // 回転方向（1 or -1）
    private float flipTimer = 0f;         // 自動反転用タイマー

    Collider2D selfCollider;

    // デリゲード
    // 関数を型にするためのもの
    private delegate void ActFunc();
    // 関数の配列
    private ActFunc[] actFuncTbl;
    enum ActNo
    {
        wait,
        move,
        shot,
        num
    }
    ActNo actNo;
    int magazine = 12;
    int capacity = 0;
    // Start is called before the first frame update
    void Start()
    {
        actNo = ActNo.wait;
        actFuncTbl = new ActFunc[(int)ActNo.num];
        actFuncTbl[(int)ActNo.wait] = Wait;
        actFuncTbl[(int)ActNo.move] = Move;
        actFuncTbl[(int)ActNo.shot] = Shot;

        selfCollider = GetComponent<Collider2D>();

        NowHP = MAXHP;
        magazine = 12;
        capacity = magazine;
    }

    // プレイヤーを中心として動くときの距離
    public float radius = 3f;
    void Wait()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

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
        if (ShotingRay())
        {

            actNo = ActNo.shot;
            shotSection = ShotSection.aim;
        }
    }


    enum ShotSection
    {
        aim,
        shot,
        wait,
        reload,
        sum
    }
    ShotSection shotSection;
    Vector3 shootDirection;
    float aimTime = 0.5f;
    float timer = 0;
    float reloadTime = 2;

    void Shot()
    {
        switch (shotSection)
        {
            case ShotSection.aim:
                timer += Time.deltaTime;
                shootDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
                if (aimTime >= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                break;
            case ShotSection.shot:

                StartCoroutine("HandGun");
                shotSection = ShotSection.wait;
                break;
            case ShotSection.wait:
                Debug.Log("残弾 " + capacity);
                break;
            case ShotSection.reload:
                capacity = magazine;
                timer += Time.deltaTime;
                if (timer >= reloadTime)
                {
                    shotSection = ShotSection.aim;
                    timer = 0;
                }
                break;
            default:
                break;
        }
    }
    IEnumerator HandGun()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

            BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

            bulletCore.HitDamegeLayer = this.HitDamegeLayer;
            bulletRigit.velocity = shootDirection * bulletSpeed;
            bulletGameObject.transform.up = shootDirection;
            capacity--;
            // 弾が0未満だったら
            if (capacity <= 0)
            {
                Debug.Log("アローリ！");
                shotSection = ShotSection.reload;
                yield break;
            }
            yield return new WaitForSeconds(1 / 3);

        }
        Debug.Log("えいむにれっつごー");
        shotSection = ShotSection.aim;
        yield return null;
    }
    void Update()
    {
        actFuncTbl[(int)actNo]();
        RotationFoward();
        Debug.Log("shotsection " + shotSection);
        Debug.Log("actNo " + actNo);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    /// <Summary>
    /// オブジェクトの位置を計算するメソッドです。
    /// </Summary>
    void CalcPosition()
    {
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            direction *= -1;
            flipTimer = 0f;
        }

        //  現在の距離を取得（動的な半径）
        float radius = Vector3.Distance(transform.position, UnitCore.Instance.transform.position);

        //  次の位置を計算（XY平面で円運動）
        angle += direction * rotationSpeed * Time.deltaTime;
        Vector3 nextPos = UnitCore.Instance.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

        //  障害物の検出（Raycastでプレイヤーから次の位置方向へ）
        Vector3 directionToNext = (nextPos - transform.position).normalized;
        float checkDistance = Vector3.Distance(transform.position, nextPos);
        if (Physics.Raycast(transform.position, directionToNext, checkDistance, obstacleMask))
        {
            // 障害物に当たったら即反転
            direction *= -1;
            flipTimer = 0f;
            return; // 今回は移動しない（ぶつかり中）
        }
        transform.position = nextPos;
    }

    bool ShotingRay()
    {
        float playerDistance = Vector2.Distance(transform.position, UnitCore.Instance.transform.position);
        Vector2 playerDirection = (UnitCore.Instance.transform.position - transform.position).normalized;

        RaycastHit2D[] hits = new RaycastHit2D[10]; // 十分なサイズ
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter(); // 全部対象
        int hitCount = Physics2D.Raycast(transform.position, playerDirection, filter, hits, playerDistance);

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider != selfCollider)
            {
                // unitCoreを持ってるオブジェクトにあったたらtureを返す
                if (hits[i].collider.gameObject.TryGetComponent<UnitCore>(out var playerObject))
                {
                    return true;
                }
                break;
            }
        }

        Debug.DrawRay(transform.position, playerDirection * playerDistance, Color.red);
        return false;
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
