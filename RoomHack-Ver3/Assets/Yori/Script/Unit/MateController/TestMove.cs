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

    [SerializeField, Header("�n�b�L���O���C�̎n�_")]
    private Transform rayStartTransform;

    [SerializeField, Header("�n�b�L���O���C�̒���")]
    private float rayDirecition;

    private Vector3 direction;

    [SerializeField, Header("�n�b�N�ł���Ώۂ̐�")]
    private int hackParallelism;

    [SerializeField, Header("�v���C���[�X�s�[�h")]
    private float MOVESPEED = 10;
    [SerializeField, Header("�}�K�W���e��")]
    private int MAXMAGAZINE;
    private int nowMagazine;
    private enum ShotMode
    {
        GunMode,
        HackMode,
        ReloadMode,
    }

    ShotMode shotMode;

    // �n�b�L���O�X�e�[�^�X
    private int hackSpeed = 1;
    //private int hackRecast = 1;
    //private int hackRam = 10;
    //private int hackRecover = 1;
    private int hackDamage = 10;
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
        playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue(), MOVESPEED);
        PlayerRotation();

        switch (shotMode)
        {
            case ShotMode.GunMode:
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
                    Debug.Log("�ؑ�" + shotMode);
                    shotMode = ShotMode.HackMode;
                }

                break;
            case ShotMode.HackMode:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Hack();
                }

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Debug.Log("�ؑ�" + shotMode);
                    shotMode = ShotMode.GunMode;
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
                Debug.LogError("�͈͂��o�����\n ShotMode:" + shotMode);
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

    [SerializeField, Header("�e�̃v���n�u")]
    private GameObject bulletPrefab;
    [SerializeField, Header("�e�̃X�s�[�h")]
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
        // ���˃��[�g��ݒ肵���̌�A���˕b�������肷��B
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

        // �������Ƀ��C���΂��i����10�j
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, 10f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject))
            {
                if (!hackObject.hacked)
                {
                    hackObject.HackStart(hackSpeed, hackDamage);
                }
                Debug.Log("�n�b�N�ł���I�u�W�F�N�g : " + hit.collider.name+" �ɂ�����܂���");
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
        Handles.Label(transform.position + Vector3.up * 1.5f, "�c�e " + nowMagazine.ToString(), style);
    }
#endif

    public void Die()
    {
        Destroy(gameObject);
    }
}
