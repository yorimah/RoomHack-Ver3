using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySecurityDrone : Enemy
{
    void Awake()
    {
        playerCheack = new PlayerCheack();

        nowMagazine = gundata.MaxMagazine;

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new DroneMoveState(this) },
        { StateType.Shot, new ShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Move;
        currentState = states[statetype];
    }
}
