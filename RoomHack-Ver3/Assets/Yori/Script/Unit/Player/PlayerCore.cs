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
    IReadPosition readPosition;

    [Inject]
    ISetPlayerDied setPlayerDied;

    public float MaxHitPoint { get; private set; }
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; private set; } = 1;

    PlayerStateContoller playerStateContoller;

    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerStateContoller = new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
            10, gameObject, playerInput);
    }

    public void Update()
    {
        readPosition.SetPlayerPosition(this.transform);
    }

    public void Die()
    {
        playerStateContoller.DieChangeState();
        setPlayerDied.SetDied();
    }
}
