using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialForce : Enemy
{
    public bool escapeMode;
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new SpecialForceMoveState(this) },
        { StateType.Shot, new SpecialForceShotState(this) },
        { StateType.Reload, new SpecialForceReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<toolTag> { toolTag.CCTVHack, toolTag.Bind, toolTag.Blind, toolTag.OverHeat, toolTag.Detonation };
    }

}
