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
    IGetMaxHitPoint getMaxHitPoint;

    [Inject]
    IHaveGun haveGun;

    [Inject]
    IGetGunData gunData;

    PlayerStateContoller playerStateContoller;

    Rigidbody2D playerRigidBody;

    public float MaxHitPoint { get; private set; }
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; private set; } = 1;
    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerStateContoller = new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
            10, gameObject, playerInput, haveGun);
        MaxHitPoint = getMaxHitPoint.maxHitPoint;
        NowHitPoint = getMaxHitPoint.maxHitPoint;
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
}
