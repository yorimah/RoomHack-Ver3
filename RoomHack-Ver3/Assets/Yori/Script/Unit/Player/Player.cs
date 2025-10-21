using System;
using System.Collections.Generic;
using UnityEngine;

public enum GunNo
{
    HandGun,
    AssultRifle,
    SniperRifle,
    SubMachineGun
}
public class Player : MonoBehaviour, IHackObject
{
    public float maxHitPoint { get; set; }
    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 1;

    // ハッキング初期化
    public List<toolTag> canHackToolTag { get; set; }
    public List<ToolEvent> nowHackEvent { get; set; }


    public int maxBullet;
    public int nowBullet;

    public PlayerSaveData data { get; private set; }

    // ハックデータ
    public float ramCapacity;
    public float nowRam;
    public float ramRecovary;

    public PlayerInput playerInput;
    // ガンデータ
    [SerializeField, Header("銃")]
    private List<GunData> gundata = new List<GunData>();
    [HideInInspector]
    public float aimTime = 0.5f;
    [HideInInspector]
    public int shotRate;
    [HideInInspector]
    public float bulletSpeed;
    [HideInInspector]
    public int stoppingPower;
    [HideInInspector]
    public float shotIntervalTime;
    [HideInInspector]
    public float reloadTime;
    [HideInInspector]
    public float maxDiffusionRate = 15;
    [HideInInspector]
    public float minDiffusionRate = 1;
    [HideInInspector]
    public float recoil;

    [SerializeField, Header("弾")]
    public GameObject bulletPrefab;


    [HideInInspector]
    public float moveSpeed;

    private Rigidbody2D rb;

    private GunNo gunNo;
    public enum StateType
    {
        Action,
        Hack,
        num
    }

    //public IState currentState;
    //public StateType stateType;
    //public Dictionary<StateType, IState> states;

    [SerializeField]
    public Material shotRanageMaterial;

    void Update()
    {
        //currentState?.Execute();

        //RamUpdate();

        // タイムスケールに応じて速度を落とす。
        //rb.linearVelocity = rb.linearVelocity * GameTimer.Instance.customTimeScale;
    }

    public void ChangeState(StateType type)
    {
        //currentState?.Exit();
        //currentState = states[type];
        //currentState?.Enter();

        //stateType = type;
    }
    void Awake()
    {
    //    PlayerDataInit();

    //    GunDataInit();

    //    // Singletonチェック
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject); // 重複を防止
    //        return;
    //    }

    //    Instance = this;

    //    states = new Dictionary<StateType, IState>()
    //{
    //    { StateType.Action, new PlayerActionState(this) },
    //    { StateType.Hack, new PlayerHackState(this) },
    //};
    //    stateType = StateType.Action;
    //    currentState = states[stateType];

    //    rb = GetComponent<Rigidbody2D>();

    //    // ハッキング初期化
    //    canHackToolTag = new List<toolTag> { toolTag.EdgeRun, toolTag.Blink};
    //    nowHackEvent = new List<ToolEvent>();
    }

    private void PlayerDataInit()
    {
        //data = SaveManager.Instance.Load();

        //maxHitPoint = data.maxHitPoint;
        //nowHitPoint = maxHitPoint;

        //ramCapacity = data.maxRamCapacity;
        //nowRam = ramCapacity;
        //ramRecovary = data.RamRecovery;

        //playerInput = new PlayerInput();
        //playerInput.Init();
        //moveSpeed = data.moveSpeed;

    }
    private void GunDataInit()
    {
        //gunNo = (GunNo)data.gunNo;
        //if (!gundata[(int)gunNo])
        //{
        //    Debug.LogError("そのような獲物はございません");
        //}
        //else
        //{
        //    shotRate = gundata[(int)gunNo].rate;
        //    shotIntervalTime = 1f / shotRate;
        //    maxBullet = gundata[(int)gunNo].MAXMAGAZINE;
        //    nowBullet = maxBullet;
        //    bulletSpeed = gundata[(int)gunNo].bulletSpeed;
        //    stoppingPower = gundata[(int)gunNo].power;
        //    reloadTime = gundata[(int)gunNo].reloadTime;
        //    recoil = gundata[(int)gunNo].recoil;
        //}
    }
    public void Die()
    {
        
    }

    // Ram回復系 by koko
    // Update呼び出し
    public bool isRebooting = false;
    public float rebootTimer { get; set; } = 0;
    private void RamUpdate()
    {
        //if (isRebooting)
        //{
        //    nowRam = 0;
        //    rebootTimer += GameTimer.Instance.ScaledDeltaTime;

        //    if (rebootTimer >= ramRecovary)
        //    {
        //        nowRam = ramCapacity;

        //        rebootTimer = 0;
        //        isRebooting = false;
        //    }
        //}
    }

    //特殊行動系
    public enum SpecialAction
    {
        none,
        EdgeRun,
        Blink
    }

    public SpecialAction nowSpecialAction = SpecialAction.none;
    public float specialActionCount = 0;
}
