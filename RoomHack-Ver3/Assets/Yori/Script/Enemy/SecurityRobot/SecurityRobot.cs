using System.Collections.Generic;
using UnityEngine;
public class SecurityRobot : EnemyBase
{
    [SerializeField]
    Material viewRange;
    void Start()
    {
        playerCheck = new PlayerCheck();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new SecurityRobotIdleState(this,viewRange) },
        { EnemyStateType.Move, new SecurityRobotMoveState(this) },
        { EnemyStateType.Shot, new SecurityRobotShotState(this) },
        { EnemyStateType.Reload, new ReloadState(this) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        cantHackToolType = new List<ToolType> { };
        nowHackEvent = new List<ToolEventBase>();
    }
}
