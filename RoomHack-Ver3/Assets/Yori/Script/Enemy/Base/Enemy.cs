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
    public List<ToolType> cantHackToolType { get; set; } = new List<ToolType>();
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
    public PlayerCheck playerCheck;

    [SerializeField, Header("予備動作")]
    public float aimTime = 0.5f;


    public float moveCoefficient = 1;
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

    public int armorInt { get; set; }

    [SerializeField, Header("装甲")]
    private int armorSerialze = 0;

    [SerializeField, Header("MaxHP")]
    private int SerializeMaxHp;

    private Rigidbody2D rigidbody;
    public void Awake()
    {
        GunDataInit();

        rigidbody = GetComponent<Rigidbody2D>();

        MaxHitPoint = SerializeMaxHp;
        if (MaxHitPoint <= 0)
        {
            MaxHitPoint = 5;
            Debug.Log("HP設定が0！ :" + gameObject.name);
        }
        NowHitPoint = MaxHitPoint;

        setEnemeyList.EnemyListAdd(this);

        HackObjectName = GetType().Name;

        armorInt = armorSerialze;
    }

    protected EnemyStateType statetype;
    protected Dictionary<EnemyStateType, IEnemyState> states;

    public float diffusionRate;

    void Update()
    {
        currentState?.Execute();
        rigidbody.linearVelocity *= moveCoefficient;
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
        // 防護点上回ってHP回復したわバカタレ
        int damage = dmg - armorInt;
        if (damage < 0) damage = 0;
        NowHitPoint -= damage;
        EffectManager.Instance.ActEffect_Num(damage, this.transform.position, 1);

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

    public void HackDmg(int dmg, float hitStop)
    {
        // ハックダメージに防護点計算してたら意味ないやんけ！おい！
        NowHitPoint -= dmg;
        EffectManager.Instance.ActEffect_Num(dmg, this.transform.position, 1);

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
