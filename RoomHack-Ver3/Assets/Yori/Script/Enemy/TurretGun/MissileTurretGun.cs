using System.Collections.Generic;
using UnityEngine;

public class MissileTurretGun : Enemy
{
    public void Start()
    {
        moveSpeed = 0;
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new TurretGunMoveState(this) },
        { StateType.Shot, new MissileShotState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];
    }
}
