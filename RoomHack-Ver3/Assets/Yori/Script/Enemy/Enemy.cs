using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; } = 5;
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 2;

    protected IState currentState;

    [SerializeField, Header("動く速さ")]
    public float moveSpeed = 3f;

    [SerializeField, Header("障害物に使うレイヤー")]
    private LayerMask obstacleMask;

    [SerializeField, Header("GunData")]
    public GunData gundata;

    [SerializeField,Header("HP")]
    private float  MaxHP;
    public PlayerCheack playerCheack;

    [HideInInspector]
    public float aimTime = 0.5f;
    [HideInInspector]
    public int MaxMagazine;
    [HideInInspector]
    public int nowMagazine;
    [HideInInspector]
    public int shotRate;
    [HideInInspector]
    public float bulletSpeed;
    public enum StateType
    {
        Idle,
        Move,
        Shot,
        Reload,
        Die,
        num
    }

    public void Start()
    {
        MAXHP = MaxHP;
        NowHP = MAXHP;
        shotRate = gundata.rate;
        MaxMagazine = gundata.MaxMagazine;
        nowMagazine = MaxMagazine;
        bulletSpeed = gundata.bulletSpeed;
    }

    public StateType statetype;
    public Dictionary<StateType, IState> states;

    void Update()
    {
        currentState?.Execute();
        Debug.Log(gameObject.name + "は" + currentState.ToString() + "を実行中");
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
        ChangeState(StateType.Die);
    }
}
