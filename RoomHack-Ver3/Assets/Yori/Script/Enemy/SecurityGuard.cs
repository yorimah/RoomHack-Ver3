using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : MonoBehaviour
{
    private IState currentState;
    public float moveSpeed = 3f;

    [SerializeField, Header("障害物に使うレイヤー")]
    private LayerMask obstacleMask;

    public enum StateType
    {
        Idle,
        Move,
        Shot,
        Reload,
        num
    }
    public StateType statetype;
    private Dictionary<StateType, IState> states;

    private Rigidbody2D secRigidBody;

    void Awake()
    {
        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new MoveState(this) },
        { StateType.Shot, new ShotState(this) }
    };
        statetype = StateType.Idle;
        currentState = states[statetype];

        secRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currentState?.Execute();
        Debug.Log(currentState.ToString() + "中");
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

    public Rigidbody2D GetRigidbody2D()
    {
        return secRigidBody;
    }
}
