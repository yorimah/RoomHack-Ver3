using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum StateType
{
    Idle,
    Move,
    Shot,
    Reload,
    Die,
    Clack,
    num
}
public class Enemy : MonoBehaviour, IDamageable, IHackObject
{
    // ハック関連
    public List<toolTag> canHackToolTag { get; set; } = new List<toolTag>();
    public List<ToolEvent> nowHackEvent { get; set; } = new List<ToolEvent>();

    // ダメージ関連
    public float maxHitPoint { get; private set; } = 5;
    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 2;

    public IState currentState;

    [SerializeField, Header("動く速さ")]
    public float moveSpeed = 3f;

    [SerializeField, Header("障害物に使うレイヤー")]
    private LayerMask obstacleMask;

    // プレイヤーとの間に障害物があるか判別するスクリプト
    public PlayerCheack playerCheack;

    [SerializeField, Header("予備動作")]
    public float aimTime = 0.5f;

    // GUNDATA
    [SerializeField, Header("GunData")]
    public GunData gunData;
    [HideInInspector]
    public int MAXBULLET;
    [HideInInspector]
    public int NOWBULLET;
    [HideInInspector]
    public int shotRate;
    [HideInInspector]
    public float bulletSpeed;
    [HideInInspector]
    public int stoppingPower;
    [HideInInspector]
    public float recoil;
    [HideInInspector]
    public float maxDiffusionRate = 15;
    [HideInInspector]
    public float minDiffusionRate = 1;

    [SerializeField, Header("弾")]
    public GameObject bulletObject;
    // 死亡フラグ
    public bool died = false;

    public float shotIntervalTime;

    public void Awake()
    {
        GunDataInit();

        nowHitPoint = maxHitPoint;
    }

    protected StateType statetype;
    protected Dictionary<StateType, IState> states;

    public float diffusionRate;

    void Update()
    {
        currentState?.Execute();

        diffusionRate = Mathf.Clamp(diffusionRate, minDiffusionRate, maxDiffusionRate);
        diffusionRate -= diffusionRate * GameTimer.Instance.ScaledDeltaTime;
    }

    public void ChangeState(StateType type)
    {
        currentState?.Exit();
        currentState = states[type];
        currentState?.Enter();
    }

    public LayerMask GetObstacleMask()
    {
        return obstacleMask;
    }

    public void Die()
    {
        died = true;
        ChangeState(StateType.Die);
    }

    public void GunDataInit()
    {
        shotRate = gunData.rate;
        MAXBULLET = gunData.MAXMAGAZINE;
        NOWBULLET = MAXBULLET;
        bulletSpeed = gunData.bulletSpeed;
        stoppingPower = gunData.power;
        shotIntervalTime = 1f / shotRate;
        moveSpeed += gunData.Maneuverability;
        recoil = gunData.recoil;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // デバッグ用表示
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;
        Handles.Label(transform.position + Vector3.up * 1f, "HP " + nowHitPoint.ToString(), style);
        Handles.Label(transform.position + Vector3.up * 1.5f, "残弾 " + NOWBULLET.ToString(), style);
        if (currentState != null)
        {
            Handles.Label(transform.position + Vector3.up * 2f, "実行ステート " + currentState.ToString(), style);
        }
        //Handles.Label(transform.position + Vector3.up * 2.5f, "NowFireWall " + NowFireWall.ToString(), style);
    }
#endif
}
