using System.Collections;
using UnityEngine;

public class SecurityGuard : MonoBehaviour, HackObject, IDamegeable
{
    public int secLevele { get; set; }

    public float MAXHP { get; set; } = 100;
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
    private int direction = 1;            // ��]�����i1 or -1�j
    private float flipTimer = 0f;         // �������]�p�^�C�}�[

    Collider2D selfCollider;

    // �f���Q�[�h
    // �֐����^�ɂ��邽�߂̂���
    private delegate void ActFunc();
    // �֐��̔z��
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

    // �v���C���[�𒆐S�Ƃ��ē����Ƃ��̋���
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
                Debug.Log("�c�e " + capacity);
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
            // �e��0������������
            if (capacity <= 0)
            {
                Debug.Log("�A���[���I");
                shotSection = ShotSection.reload;
                yield break;
            }
            yield return new WaitForSeconds(1 / 3);

        }
        Debug.Log("�����ނɂ�����[");
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
    /// �I�u�W�F�N�g�̈ʒu���v�Z���郁�\�b�h�ł��B
    /// </Summary>
    void CalcPosition()
    {
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            direction *= -1;
            flipTimer = 0f;
        }

        //  ���݂̋������擾�i���I�Ȕ��a�j
        float radius = Vector3.Distance(transform.position, UnitCore.Instance.transform.position);

        //  ���̈ʒu���v�Z�iXY���ʂŉ~�^���j
        angle += direction * rotationSpeed * Time.deltaTime;
        Vector3 nextPos = UnitCore.Instance.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

        //  ��Q���̌��o�iRaycast�Ńv���C���[���玟�̈ʒu�����ցj
        Vector3 directionToNext = (nextPos - transform.position).normalized;
        float checkDistance = Vector3.Distance(transform.position, nextPos);
        if (Physics.Raycast(transform.position, directionToNext, checkDistance, obstacleMask))
        {
            // ��Q���ɓ��������瑦���]
            direction *= -1;
            flipTimer = 0f;
            return; // ����͈ړ����Ȃ��i�Ԃ��蒆�j
        }
        transform.position = nextPos;
    }

    bool ShotingRay()
    {
        float playerDistance = Vector2.Distance(transform.position, UnitCore.Instance.transform.position);
        Vector2 playerDirection = (UnitCore.Instance.transform.position - transform.position).normalized;

        RaycastHit2D[] hits = new RaycastHit2D[10]; // �\���ȃT�C�Y
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter(); // �S���Ώ�
        int hitCount = Physics2D.Raycast(transform.position, playerDirection, filter, hits, playerDistance);

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider != selfCollider)
            {
                // unitCore�������Ă�I�u�W�F�N�g�ɂ���������ture��Ԃ�
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
