using UnityEngine;
using Zenject;
public class PlayerCore : MonoBehaviour, IDamageable, IReadOnlyPlayerPoision
{
    IReadOnlyPlayerStatus playerStatus;

    Rigidbody2D playerRigidBody;

    PlayerSaveData playerSaveData;
    [SerializeField]
    GunData gunData;
    [SerializeField]
    Material material;
    [SerializeField]
    GameObject bulletPre;

    [Inject]
    IPlayerInput iPlayerInput;

    public float maxHitPoint { get; private set; }
    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; private set; } = 1;

    public Vector3 PlayerPosition { get; set; }

    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSaveData = SaveManager.Instance.Load();
        playerStatus = new PlayerStatus(playerSaveData);
        new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
            10,  gameObject, iPlayerInput);
    }

    public void Die()
    {

    }
}
