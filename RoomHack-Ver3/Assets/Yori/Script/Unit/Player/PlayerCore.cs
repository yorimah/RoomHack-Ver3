using UnityEngine;
using Zenject;
public class PlayerCore : MonoBehaviour, IDamageable, IReadPosition
{
    public Vector3 PlayerPosition { get { return this.transform.position; } }
    Rigidbody2D playerRigidBody;

    [SerializeField]
    GunData gunData;
    
    [SerializeField]
    Material material;
    
    [SerializeField]
    GameObject bulletPre;


    // Null表記になっているが、外から注入するので気にしないでよい
    [Inject]
    IPlayerInput iPlayerInput;

    public float maxHitPoint { get; private set; }
    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; private set; } = 1;

    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        new PlayerStateContoller(playerRigidBody, gunData, material, bulletPre,
            10,  gameObject, iPlayerInput);
    }

    public void Die()
    {

    }
}
