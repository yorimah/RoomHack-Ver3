using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : Enemy
{
    void Awake()
    {
        playerCheack = new PlayerCheack();

        nowMagazine = gundata.MaxMagazine;

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new MoveState(this) },
        { StateType.Shot, new ShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];
    }
}
