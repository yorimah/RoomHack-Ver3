using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Zenject;
public enum EnemyStateType
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
    public List<ToolTag> canHackToolTag { get; set; } = new List<ToolTag>();
    public List<ToolEventBase> nowHackEvent { get; set; } = new List<ToolEventBase>();

    public bool CanHack { get; set; } = false;

    public bool IsView { get; set; }

    // ダメージ関連
    public float MaxHitPoint { get; private set; } = 5;
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 2;

    public IEnemyState currentState;

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
    public bool isDead = false;

    public float shotIntervalTime;

    [Inject]
    IPosition readOnlyPlayerPoision;

    [Inject]
    ISetEnemeyList setEnemeyList;

    public Vector3 PlayerPosition { get { return readOnlyPlayerPoision.PlayerPosition; } }

    public IPosition getIPosition { get { return readOnlyPlayerPoision; } }

    [Inject]
    ISetScoreDestroy setScoreDestroy;

    public string HackObjectName { get; protected set; }
    
    public void Awake()
    {
        GunDataInit();

        NowHitPoint = MaxHitPoint;

        setEnemeyList.EnemyListAdd(this);

        HackObjectName = GetType().Name;
    }

    protected EnemyStateType statetype;
    protected Dictionary<EnemyStateType, IEnemyState> states;

    public float diffusionRate;

    void Update()
    {
        currentState?.Execute();

        diffusionRate = Mathf.Clamp(diffusionRate, minDiffusionRate, maxDiffusionRate);
        diffusionRate -= diffusionRate * GameTimer.Instance.GetScaledDeltaTime();
    }

    public void ChangeState(EnemyStateType type)
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
        setScoreDestroy.AddScoreDestroy(1);
        //setEnemeyList.EnemyListRemove(this);
        isDead = true;
        ChangeState(EnemyStateType.Die);
    }

    public void GunDataInit()
    {
        shotRate = gunData.Rate;
        MAXBULLET = gunData.MaxBullet;
        NOWBULLET = MAXBULLET;
        bulletSpeed = gunData.BulletSpeed;
        stoppingPower = gunData.Power;
        shotIntervalTime = 1f / shotRate;
        moveSpeed += gunData.Maneuverability;
        recoil = gunData.Recoil;
    }
    public void HitDmg(int dmg, float hitStop)
    {
        NowHitPoint -= dmg;
        if (NowHitPoint <= 0)
        {
            Die();
        }
        else
        {
            if (hitDamegeLayer == 2)
            {
                SeManager.Instance.Play("Hit");
            }
            HitStopper.Instance.StopTime(hitStop);
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // デバッグ用表示
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;
        Handles.Label(transform.position + Vector3.up * 1f, "HP " + NowHitPoint.ToString(), style);
        Handles.Label(transform.position + Vector3.up * 1.5f, "残弾 " + NOWBULLET.ToString(), style);
        if (currentState != null)
        {
            Handles.Label(transform.position + Vector3.up * 2f, "実行ステート " + currentState.ToString(), style);
        }
        Handles.Label(transform.position + Vector3.up * 2.5f, "CanHack " + CanHack.ToString(), style);
    }
#endif
}
