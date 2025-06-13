using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : MonoBehaviour
{
    private IState currentState;
    public float moveSpeed = 3f;

    [SerializeField, Header("障害物に使うレイヤー")]
    private LayerMask obstacleMask;

    enum StateType
    {
        Idle,
        Move,
        Shot,
        Reload,
        num
    }
    StateType statetype;
    private Dictionary<StateType, IState> states;

    void Start()
    {
        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new MoveState(this) },
        { StateType.Shot, new ShotState(this) }
    };
        statetype = StateType.Idle;
    }

    void Update()
    {
        currentState?.Execute();
        //Debug.Log(currentState.ToString() + "中");
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = states[statetype];
        currentState?.Enter();
    }

    public LayerMask GetObstacleMask()
    {
        return obstacleMask;
    }
}
