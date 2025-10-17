﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialForce : Enemy
{
    public bool escapeMode;
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new IdleState(this) },
        { EnemyStateType.Move, new SpecialForceMoveState(this) },
        { EnemyStateType.Shot, new SpecialForceShotState(this) },
        { EnemyStateType.Reload, new SpecialForceReloadState(this) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<toolTag> { toolTag.CCTVHack, toolTag.Bind, toolTag.Blind, toolTag.OverHeat, toolTag.Detonation };
    }

}
