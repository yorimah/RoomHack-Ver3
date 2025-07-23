using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySecurityDrone : Enemy
{
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new DroneMoveState(this) },
        { StateType.Shot, new DroneShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Clack, new DroneClackState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];
    }
}
