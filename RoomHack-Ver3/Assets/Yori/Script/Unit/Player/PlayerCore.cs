using UnityEngine;
using Zenject;

public class PlayerCore : MonoBehaviour, IDamageable
{
    [SerializeField]
    Material material;

    [SerializeField]
    GameObject bulletPre;

    [Inject]
    IPlayerInput playerInput;

    [Inject]
    IPosition position;

    [Inject]
    ISetPlayerDied setPlayerDied;

    [Inject]
    IGetHitPoint getHitPoint;
    [Inject]
    ISetHitPoint setHitPoint;

    [Inject]
    IHaveGun haveGun;

    [Inject]
    IGetGunData gunData;

    [Inject]
    IGetMoveSpeed getMoveSpeed;

    PlayerStateContoller playerStateContoller;

    Rigidbody2D playerRigidBody;

    public float MaxHitPoint { get; private set; }
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; private set; } = 1;

    //　セッターや初期化はawakeで行うこと
    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        MaxHitPoint = getHitPoint.MaxHitPoint;
        NowHitPoint = getHitPoint.MaxHitPoint;
    }

    public void Start()
    {
        playerStateContoller = new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
           getMoveSpeed, gameObject, playerInput, haveGun);
    }

    public void Update()
    {
        position.PlayerPositionSet(this.transform);
    }

    public void Die()
    {
        playerStateContoller.DieChangeState();
        setPlayerDied.DieSet();
    }

    public void HitDmg(int dmg, float hitStop)
    {
        setHitPoint.DamageHitPoint(dmg);
        EffectManager.Instance.ActEffect_Num(dmg, this.transform.position, 1);

        if (getHitPoint.NowHitPoint <= 0)
        {
            Die();
        }
        else
        {
            if (hitDamegeLayer == 2)
            {
                SeManager.Instance.Play("Hit");
            }
            HitStopper.Instance.StopTime(hitStop);
        }
    }
}
