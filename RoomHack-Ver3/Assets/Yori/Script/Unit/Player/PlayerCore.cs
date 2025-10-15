using UnityEngine;

public class PlayerCore : MonoBehaviour
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
    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = new();
        playerSaveData = SaveManager.Instance.Load();
        playerData = new(playerSaveData);
        new PlayerStateContoller(playerRigidBody, gunData,  material,  bulletPre, playerData.moveSpeed, playerInput,gameObject);

    }
}
