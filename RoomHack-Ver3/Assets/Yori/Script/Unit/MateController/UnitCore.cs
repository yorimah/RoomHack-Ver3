using UnityEngine;
#if UNITY_EDITOR
#endif
using UnityEngine.SceneManagement;
using System.Threading;
using System.Collections.Generic;
public class UnitCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 1;

    public static UnitCore Instance { get; private set; }
    public int MAXBULLET { get; private set; }
    public int NOWBULLET { get; set; }
    public PlayerSaveData data { get; private set; }

    // ハックデータ
    public float ramCapacity;
    public float nowRam;
    public float ramRecovary;

    // プレイヤー初期値
    private int initMaxHp = 100;
    private int initRamCapacity = 10;
    private int initRamRecovary = 1;

    public MoveInput moveInput;
    // ガンデータ
    [SerializeField, Header("GunData")]
    public GunData gundata;
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

    [SerializeField, Header("弾")]
    public GameObject bulletPrefab;
    public enum StateType
    {
        Action,
        Hack,
        Die,
        num
    }

    public IState currentState;
    public StateType statetype;
    public Dictionary<StateType, IState> states;
    void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(StateType type)
    {
        currentState?.Exit();
        currentState = states[type];
        currentState?.Enter();
    }
    void Awake()
    {
        data = SaveManager.Instance.Load();

        MAXHP = initMaxHp + data.pulusMaxHitpoint;
        NowHP = MAXHP;

        GunDataInit();

        ramCapacity = initRamCapacity + data.plusRamCapacity;
        nowRam = ramCapacity;
        ramRecovary = initRamRecovary + data.plusRamRecovery;

        moveInput = new MoveInput();
        moveInput.Init();

        // Singletonチェック
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複を防止
            return;
        }

        Instance = this;

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Action, new PlayerActionState(this) },
        { StateType.Hack, new PlayerHackState(this) },

        //{ StateType.Die, new DieState(this) },
    };
        statetype = StateType.Action;
        currentState = states[statetype];
    }

    private void GunDataInit()
    {
        shotRate = gundata.rate;
        MAXBULLET = gundata.MaxMagazine;
        NOWBULLET = MAXBULLET;
        bulletSpeed = gundata.bulletSpeed;
        stoppingPower = gundata.power;
        shotIntervalTime = 1f / shotRate;
        reloadTime = gundata.reload;
    }
    public void Die()
    {
        SceneManager.LoadScene("GameOverDemoScene");
    }
}
