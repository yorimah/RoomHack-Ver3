using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
        setHitPoint.SetNowHitPoint(NowHitPoint);
    }

    public void Die()
    {
        playerStateContoller.DieChangeState();
        setPlayerDied.DieSet();
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        // デバッグ用表示
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;
        Handles.Label(transform.position + Vector3.up * 1f, "HP " + NowHitPoint.ToString(), style);
    }
#endif
}
