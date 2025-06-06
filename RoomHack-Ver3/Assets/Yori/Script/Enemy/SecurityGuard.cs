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

    [SerializeField, Header("�e�̃v���n�u")]
    private GameObject bulletPrefab;
    [SerializeField, Header("�e�̃X�s�[�h")]
    private float bulletSpeed;

    bool isInside;

    public float rotationSpeed = 1f;      // ��]���x�i���W�A��/�b�j
    public float flipInterval = 1f;       // �������]�̎���
    public LayerMask obstacleMask;        // ��Q���Ɏg�����C���[

    private float angle = 0f;             // ���݂̊p�x
    private float direction = 1;
    private float flipTimer = 0f;         // �������]�p�^�C�}�[

    // �f���Q�[�h
    // �֐����^�ɂ��邽�߂̂���
    private delegate void ActFunc();
    // �֐��̔z��
    private ActFunc[] actFuncTbl;

    ShotSection shotSection;
    Vector3 shootDirection;
    float aimTime = 0.5f;
    float timer = 0;
    float reloadTime = 2;


    private Rigidbody2D secRididBody;
    // �����[�h�������ɒǉ�
    enum ActNo
    {
        wait,
        move,
        shot,
        reload,
        num
    }
    ActNo actNo;

    // �V���b�g�֘A
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

    // �v���C���[�𒆐S�Ƃ��ē����Ƃ��̋���
    public float radius = 3f;
    void Wait()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // ��ʓ�����@��������true
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
            // �V���b�g�Ɉړ�
            actNo = ActNo.shot;
            timer = 0;
        }
    }
    enum ShotSection
    {
        aim,
        shot,
        // ���O�ύX��]
        shotInterval,
        sum
    }


    void Shot()
    {
        // ���˃��[�g��ݒ肵���̌�A���˕b�������肷��B
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

        // �e��0�����������烊���[�h�ɑJ��
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
    /// �I�u�W�F�N�g�̈ʒu���v�Z���郁�\�b�h�ł��B
    /// </Summary>
    void CalcPosition()
    {
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            direction = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
        }

        // �v���C���[�Ƃ̋����i���I�Ȕ��a�j
        Vector2 center = UnitCore.Instance.transform.position;
        Vector2 dir = (Vector2)transform.position - center;

        emDir = new Vector2(-dir.y, dir.x);
        nextPos = (Vector2)transform.position + (emDir * direction);

        // ��Q���`�F�b�N
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

        // Rigidbody2D �ňړ�
        secRididBody.velocity = directionToNext.normalized * 5;
    }

    /// <Summary>
    /// ���C���΂��ĕǂɂ���������false������Ȃ�������true
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
