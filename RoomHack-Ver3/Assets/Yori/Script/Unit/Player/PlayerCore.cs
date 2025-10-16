using UnityEngine;

public class PlayerCore : MonoBehaviour, IDamageable
{
    PlayerData playerData;

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

    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = new PlayerInput();
        playerSaveData = SaveManager.Instance.Load();
        playerData = new PlayerData(playerSaveData);
        new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
            playerData.moveSpeed, playerInput, gameObject);
    }

    public void Die()
    {

    }
}
