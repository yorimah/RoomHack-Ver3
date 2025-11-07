using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
public class PlayerCore : MonoBehaviour, IDamageable
{
    Rigidbody2D playerRigidBody;

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

    public float MaxHitPoint { get; private set; }
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; private set; } = 1;

    [Inject]
    IGetMaxHitPoint getMaxHitPoint;

    [Inject]
    IHaveGun haveGun;

    PlayerStateContoller playerStateContoller;

    [Inject]
    IGetGunData gunData;
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
