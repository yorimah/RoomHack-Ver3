using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; } = 5;
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 2;
    private IState currentState;
    public float moveSpeed = 3f;

    [SerializeField, Header("障害物に使うレイヤー")]
    private LayerMask obstacleMask;

    [SerializeField, Header("GunData")]
    public GunData gundata;
    public enum StateType
    {
        Idle,
        Move,
        Shot,
        Reload,
        Die,
        num
    }
    public StateType statetype;
    private Dictionary<StateType, IState> states;

    void Awake()
    {
        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new MoveState(this) },
        { StateType.Shot, new ShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Shot;
        currentState = states[statetype];


    }

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
