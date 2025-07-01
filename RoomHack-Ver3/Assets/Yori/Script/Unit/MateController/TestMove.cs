using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class TestMove : MonoBehaviour
{
    private MoveInput moveInput;

    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private Vector3 mousePosition;

    [SerializeField, Header("ハッキングレイの始点")]
    private Transform rayStartTransform;

    [SerializeField, Header("ハッキングレイの長さ")]
    private float rayDirecition;

    private Vector3 direction;

    [SerializeField, Header("プレイヤースピード")]
    private float MOVESPEED = 10;
    [SerializeField, Header("マガジン容量")]
    private int MAXMAGAZINE;
    private int nowMagazine;
    private enum ShotMode
    {
        GunMode,
        HackMode,
        ReloadMode,
    }

    ShotMode shotMode;

    // ハッキングステータス
    [SerializeField, Header("ぶりーちぱわー")]
    private float breachPower;

    public void Start()
    {
        moveInput = new MoveInput();

        moveInput.Init();

        playerRigidbody2D = this.GetComponent<Rigidbody2D>();

        nowMagazine = MAXMAGAZINE;
    }
    private float reloadTime = 2;
    public void Update()
    {
        playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue(), MOVESPEED) * GameTimer.Instance.customTimeScale;
        PlayerRotation();

        switch (shotMode)
        {
            case ShotMode.GunMode:
                GameTimer.Instance.SetTimeScale(1);
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (nowMagazine > 0)
                    {
                        Shot();
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    timer = 0;
                    shotMode = ShotMode.ReloadMode;
                }

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    shotMode = ShotMode.HackMode;
                    Debug.Log("切替" + shotMode);
                }

                break;
            case ShotMode.HackMode:
                GameTimer.Instance.SetTimeScale(0.1f);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Hack();
                }

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    shotMode = ShotMode.GunMode;
                    Debug.Log("切替" + shotMode);
                }
                break;
            case ShotMode.ReloadMode:
                timer += Time.deltaTime;
                if (reloadTime <= timer)
                {
                    Reload();
                    shotMode = ShotMode.GunMode;
                }

                break;
            default:
                Debug.LogError("範囲を出たわよ\n ShotMode:" + shotMode);
                break;
        }
    }
    private void Reload()
    {
        nowMagazine = MAXMAGAZINE;
    }
    public Vector2 PlayerMoveVector(Vector2 inputMoveVector, float moveSpeed)
    {
        moveVector = inputMoveVector * moveSpeed;
        return moveVector;
    }

    private void PlayerRotation()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - this.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;
    }

    [SerializeField, Header("弾のプレハブ")]
    private GameObject bulletPrefab;
    [SerializeField, Header("弾のスピード")]
    private float bulletSpeed;

    private void GunFire()
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        bulletCore.power = 40;
        bulletCore.hitStop = 0.1f;
        bulletCore.HitDamegeLayer = UnitCore.Instance.HitDamegeLayer;

        Vector3 shootDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
        bulletRigit.velocity = shootDirection * bulletSpeed;
        bulletGameObject.transform.up = shootDirection;
    }

    enum ShotSection
    {
        shot,
        shotInterval,
        sum
    }

    private ShotSection shotSection;
    float timer = 0;


    float shotIntevalTime = 1 / 3f;
    void Shot()
    {
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.shot:
                GunFire();
                nowMagazine--;
                shotSection++;
                break;
            case ShotSection.shotInterval:

                timer += Time.deltaTime;
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
    private void Hack()
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 下方向にレイを飛ばす（距離10）
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, 10f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject))
            {
                if (!hackObject.clacked)
                {
                    hackObject.Clack(breachPower);
                }
                Debug.Log("ハックできるオブジェクト : " + hit.collider.name + " にあたりました");
            }
        }

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;

        if (UnitCore.Instance != null)
        {
            Handles.Label(transform.position + Vector3.up * 1f, "HP " + UnitCore.Instance.NowHP.ToString(), style);
        }
        Handles.Label(transform.position + Vector3.up * 1.5f, "残弾 " + nowMagazine.ToString(), style);
    }
#endif

    public void Die()
    {
        Destroy(gameObject);
    }
}
