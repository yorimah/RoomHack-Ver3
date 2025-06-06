using UnityEngine;

public class TestMove : MonoBehaviour
{

    private MoveInput moveInput;

    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private float MOVESPEED = 10;

    private Vector3 mousePosition;

    [SerializeField, Header("�n�b�L���O���C�̎n�_")]
    private Transform rayStartTransform;

    [SerializeField, Header("�n�b�L���O���C�̒���")]
    private float rayDirecition;

    private Vector3 direction;

    [SerializeField, Header("�n�b�N�X�s�[�h")]
    private int hackSpeedLevel;

    [SerializeField, Header("�n�b�N���Ώۂɉe����^���鎞��")]
    private float hackDamage;

    [SerializeField, Header("�n�b�N�ł���Ώۂ̐�")]
    private int hackParallelism;


    private enum ShotMode
    {
        GunMode,
        HackMode,
    }

    ShotMode shotMode;
    public void Start()
    {
        moveInput = new MoveInput();

        moveInput.Init();

        playerRigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue(), MOVESPEED);
        PlayerRotation();

        switch (shotMode)
        {
            case ShotMode.GunMode:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Shot();
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
            default:
                Debug.LogError("�͈͂��o�����\n ShotMode:" + shotMode);
                break;
        }
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

    private void Shot()
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        bulletCore.HitDamegeLayer = UnitCore.Instance.HitDamegeLayer;

        Vector3 shootDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
        bulletRigit.velocity = shootDirection * bulletSpeed;
        bulletGameObject.transform.up = shootDirection;
    }

    private void Hack()
    {
        RaycastHit2D ray = Physics2D.Raycast(rayStartTransform.position, direction, rayDirecition);
        Debug.DrawRay(rayStartTransform.position, direction * rayDirecition, Color.red);

        if (ray.collider != null)
        {
            Debug.Log(ray.collider.gameObject.name);
            if (ray.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject))
            {
                //hackObject.HackStart();

            }
        }
    }



    public void Die()
    {
        Destroy(gameObject);
    }
}
