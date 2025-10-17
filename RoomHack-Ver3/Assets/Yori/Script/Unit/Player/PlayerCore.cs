using UnityEngine;

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
    PlayerInput playerInput;

    public float maxHitPoint { get; private set; }
    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; private set; } = 1;

    public Vector3 PlayerPosition { get; set; }

    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = new PlayerInput();
        playerSaveData = SaveManager.Instance.Load();
        playerStatus = new PlayerStatus(playerSaveData);
        new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
            10, playerInput, gameObject);
        Debug.Log("Maxhp" + playerStatus.MaxHitPoint);
    }

    public void Die()
    {

    }
}
