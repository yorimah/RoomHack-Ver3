using System.Collections.Generic;

public class SecurityRobot : EnemyBase
{
    void Start()
    {
        playerCheck = new PlayerCheck();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new IdleState(this) },
        { EnemyStateType.Move, new SecurityRobotMoveState(this) },
        { EnemyStateType.Shot, new ShotState(this) },
        { EnemyStateType.Reload, new ReloadState(this) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        cantHackToolType = new List<ToolType> { };
        nowHackEvent = new List<ToolEventBase>();
    }
}
