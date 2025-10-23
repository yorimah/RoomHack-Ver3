using UnityEngine;
using Zenject;
public class PlayerCore : MonoBehaviour, IDamageable
{
    Rigidbody2D playerRigidBody;

    [SerializeField]
    GunData gunData;

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
    IReadMaxHitPoint readMaxHitPoint;

    PlayerStateContoller playerStateContoller;

    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerStateContoller = new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
            10, gameObject, playerInput);
        MaxHitPoint = readMaxHitPoint.MaxHitPoint;
        NowHitPoint = readMaxHitPoint.MaxHitPoint;
    }

    public void Update()
    {
        position.SetPlayerPosition(this.transform);
    }

    public void Die()
    {
        playerStateContoller.DieChangeState();
        setPlayerDied.SetDied();
    }
}
